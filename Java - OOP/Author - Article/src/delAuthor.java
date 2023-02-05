
import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;

public class delAuthor {
    public void delAuthors(String id) throws IOException {
        Writer writer = new BufferedWriter(new OutputStreamWriter(new FileOutputStream("output.txt", true), StandardCharsets.UTF_8));
        writer.write("----------------------------------------list----------------------------------------");
        output output = new output();
        ArrayList<String> outputFile;
        outputFile = output.getFile();
        ArrayList<String> currentList = new ArrayList<>();
        boolean checkSS = false;
        for (int line = outputFile.size(); line > 0; line--) {

            if (outputFile.get(line - 1).contains("end")) { checkSS = true; }
            else if (outputFile.get(line-2).contains("list")) { break; }

            if (checkSS) { currentList.add(outputFile.get(line - 2)); }

        }
        boolean myBool = false;
        for (int line = currentList.size() - 1; line > 0; line--) {
            if(currentList.get(line).contains(id)){
                myBool = true;
            }
            else if(currentList.get(line).contains("Author:")){
                myBool = false;
            }

            if(!myBool){
                writer.write("\n"+currentList.get(line));
            }


        }
        writer.write("\n" );
        writer.write("----------------------------------------end-----------------------------------------\n");
        writer.close();
    }

}
