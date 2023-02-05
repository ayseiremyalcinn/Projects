import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.*;

public class Main {

    public static void main(String[] args) throws IOException {

        File file = new File();
        ArrayList<String> parts =  file.readFile( args[0]);
        ArrayList<String> items =  file.readFile(args[1]);
        ArrayList<String> tokenss =  file.readFile(args[2]);
        ArrayList<String> tasks =  file.readFile(args[3]);

        LinkedHashMap<String, Stack> vendingMac = new LinkedHashMap<>();
        for (String part: parts){
            Stack stack = new Stack();
            for(String item: items){
                if(item.split(" ")[1].equals(part)){
                    stack.push(item.split(" ")[0]);
                }
            }
            vendingMac.put(part, stack);
        }
        Queue tokens = new Queue();
        for (String line: tokenss) {
            String[] tmp = line.split(" ");
            Tokens token = new Tokens(tmp[0], tmp[1], Integer.parseInt(tmp[2]));
            tokens.enQueue(token);
        }

        //......TASKS...........
        for(String t: tasks){
            String[] task = t.split("\t");
            for (int i = 1; i < task.length ; i++) {
                String[] list =  task[i].split(",");
                if(task[0].equals("PUT")){
                    for (int j = 1; j < list.length ; j++) {
                        vendingMac.get(list[0]).push(list[j]);
                    }
                }
                else if(task[0].equals("BUY")){

                    Tokens maxToken = tokens.findMaxToken(list[0]);
                    for (int j = 0; j < Integer.parseInt(list[1]); j++) {

                        vendingMac.get(list[0]).pop();
                        if(maxToken.number == 0){
                            tokens.remove(maxToken);
                            maxToken = tokens.findMaxToken(list[0]);
                            maxToken.number -= 1;
                        }else{
                            maxToken.number -= 1;
                        }
                    }
                    tokens.remove(maxToken);
                    if(maxToken.number != 0){
                        tokens.enQueue(maxToken);

                    }
//                    if(maxToken.number == 0){
//                        tokens.remove(maxToken);
//                    }else{
//
//                        tokens.enQueue(maxToken);
//
//                    }
                }
            }
        }

        //......OUTPUT.........
        BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(new FileOutputStream(args[4], true), StandardCharsets.UTF_8));
        for (String part: parts) {
            writer.write(part +":\n");
            for (int j = vendingMac.get(part).stack.length -1; j >= 0; j--) {
                if(vendingMac.get(part).stack[j] != null){
                    writer.write(vendingMac.get(part).stack[j] +"\n");
                }
            }
            writer.write("---------------\n");
        }
        writer.write("Token Box:\n");
        for(Tokens i: tokens.queue){
            writer.write(i.id + " " + i.type + " " + i.number + "\n");
        }
        writer.close();
    }
}
