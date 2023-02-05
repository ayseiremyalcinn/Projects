

import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;


public class Commands {
    boolean readFile = false;
    boolean completeAll = false;
    boolean sortedAll = false;
    boolean del = false;
    public String id;

    public Commands(String input, String input1) {

        try {
            BufferedReader br = new BufferedReader(new FileReader( "src\\"+ input1));
            ArrayList<String> command_file = new ArrayList<>();

            while (true) {
                String a = br.readLine();
                if (a != null) {
                    command_file.add(a);
                } else {
                    break;
                }

            }
            ArrayList<String> articleFile = new ArrayList<>();
            for (String command : command_file) {
                Writer writer = new BufferedWriter(new OutputStreamWriter(new FileOutputStream("output.txt", true), StandardCharsets.UTF_8));
                if (command.split(" ")[0].equals("read")) {
                    Article article = new Article();
                    articleFile = article.getFile(articleFile, command.split(" ")[1]);
                    readFile = true;

                }
                else if (command.equals("list")) {
                    Author author = new Author();
                    ArrayList<String> authorFile = author.getFile(input);
                     if (completeAll) {
                        writer.write("**********************************CompleteAll Successful***************************\n");
                        writer.close();
                        List list = new List();
                        list.print(authorFile, articleFile, "completeAll", input);

                        completeAll = false;
                    } else if (sortedAll) {
                        writer.write("************************************SortedAll Successful****************************\n");
                        writer.close();
                        getOutputFile getsortedAll = new getOutputFile();
                        getsortedAll.getList();


                        sortedAll = false;
                    } else if (del) {
                        writer.write("********************************del Successful***************************************\n");
                        writer.close();
                        delAuthor delAuthor = new delAuthor();
                        delAuthor.delAuthors(id);
                        del = false;
                    }else if (readFile) {
                        List list = new List();
                        list.print(authorFile, articleFile, "readFile", input);
                        readFile = false;
                    }

                }
                else if(command.equals("completeAll")){
                    completeAll = true;
                }
                else if(command.equals("sortedAll")){
                    sortedAll = true;
                }
                else if (command.split(" ")[0].equals("del")){
                    id = command.substring(4);
                    del = true;
                }
            }
        }
        catch(IOException ex){
                ex.printStackTrace();
            }



    }
}
