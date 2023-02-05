
import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.Arrays;

public class List {

    public void print(ArrayList<String> authorFile, ArrayList<String> articleFile, String command, String input) throws IOException {

        Writer writer = new BufferedWriter(new OutputStreamWriter(new FileOutputStream("output.txt", true), StandardCharsets.UTF_8));

        writer.write("----------------------------------------list----------------------------------------\n");
        ArrayList<ArrayList<String>> newAuthor = new ArrayList<>();
        for (String person : authorFile) {

            int sayac = 0;
            writer.write("Author:");
            String[] listAuthorr = person.split(" ");
            ArrayList<String> listAuthor = new ArrayList<>();
            listAuthor.addAll(Arrays.asList(listAuthorr));
            newAuthor.add(listAuthor);
            writer.write(listAuthor.get(1) + "  " );
            int sayac1 = 0;
            for (String i : listAuthor) {
                if((!i.contains("1")) && sayac1 > 0){
                    writer.write(i+ "\t");
                }
                sayac1++;

            }
            writer.write("\n");

            if(listAuthor.size() < 7) {
                for (String article : articleFile) {
                    String[] liste = article.split(" ");
                    if ("completeAll".equals(command)) {
                        if (liste[1].startsWith(listAuthor.get(1)) && sayac < 5) {
                            writer.write("+" + liste[1] + "\t" + liste[2] + "\t" + liste[3] + "\t" + liste[4] + "\n");
                            newAuthor.get(newAuthor.size()-1).add(liste[1]);
                            sayac++;
                        }
                    }
                    else if("readFile".equals(command)){
                        int sayac2 = 0;
                        for (String i : listAuthor) {
                            if((i.contains("1")) && sayac2 > 0){
                                if(i.equals(liste[1]) && sayac < 5){
                                    writer.write("+" + liste[1] + "\t" + liste[2] + "\t" + liste[3] + "\t" + liste[4]+"\n" );
                                    sayac++;
                                }
                            }
                            sayac2++;

                        }
                    }
                }
            }
            else{
                for (int i = 6; i < listAuthor.size(); i++) {
                    for (String article : articleFile) {
                        String[] liste = article.split(" ");
                        if ("readFile".equals(command)) {
                            if (listAuthor.get(i).equals(liste[1]) && sayac < 5) {
                                writer.write("+" + liste[1] + "\t" + liste[2] + "\t" + liste[3] + "\t" + liste[4]+"\n" );
                                sayac++;
                            }
                        } else if ("completeAll".equals(command)) {
                            if (listAuthor.get(i).equals(liste[1]) && sayac < 5) {
                                writer.write("+" + liste[1] + "\t" + liste[2] + "\t" + liste[3] + "\t" + liste[4]+"\n");
                                sayac++;
                            }
                            else if(liste[1].startsWith(listAuthor.get(1)) && sayac < 5){
                                writer.write("+" + liste[1] + "\t" + liste[2] + "\t" + liste[3] + "\t" + liste[4] + "\n");
                                newAuthor.get(newAuthor.size()-1).add(liste[1]);
                                sayac++;
                            }
                        }
                    }
                }
            }
            writer.write("\n");
        }
        writer.write("----------------------------------------end-----------------------------------------\n");
        writer.close();

        if ("completeAll".equals(command)){
            Writer writer1 = new BufferedWriter(new OutputStreamWriter(new FileOutputStream("src\\"+input), StandardCharsets.UTF_8));
            writer1.write("\n");
            writer1.close();
            Writer writerAuthor = new BufferedWriter(new OutputStreamWriter(new FileOutputStream("src\\" +input), StandardCharsets.UTF_8));
            for(ArrayList<String> i:newAuthor){
                for(String k : i){
                    writerAuthor.write(k+ " ");
                }
                writerAuthor.write("\n");

            }
            writerAuthor.close();
        }

    }
}
