import java.io.*;
import java.util.ArrayList;

public class output {
    public ArrayList<String> output_file = new ArrayList<>();

    public ArrayList<String> getFile() {
        try{
            BufferedReader br = new BufferedReader(new FileReader("output.txt"));


            while(true){
                String newData = br.readLine();
                if (newData != null) { output_file.add(newData); }
                else{ break;}
            }
            return output_file;

        }
        catch (IOException ex){
            ex.printStackTrace();
        }
        return output_file;
    }
}
