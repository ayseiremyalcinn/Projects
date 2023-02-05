import java.io.*;
import java.util.ArrayList;

public class Author {
    public ArrayList<String> author_file = new ArrayList<>();

    public ArrayList<String> getFile(String input) {

        try{

            BufferedReader br = new BufferedReader(new FileReader(  input));


            while(true){
                String newData = br.readLine();
                if (newData != null) { author_file.add(newData); }
                else{ break;}
            }
            return author_file;

        }
        catch (IOException ex){
            ex.printStackTrace();
        }
        return author_file;
        }
    }








