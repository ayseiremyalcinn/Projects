
import java.io.*;

import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.Collections;


public class getOutputFile {
    public void getList() throws IOException {
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
        ArrayList<String> temp = new ArrayList<>();
        for (int k = currentList.size() - 1; k > 0; k--) {
            if (currentList.get(k).contains("+")) {
                assert false;
                temp.add(currentList.get(k));
            }
            else if(currentList.get(k).contains("Author:")) {
                assert false;
                Collections.sort(temp);
                for(String j:temp){
                    writer.write(j+ "\n");
                }
                temp.clear();
                writer.write("\n"+currentList.get(k)+"\n");

            }
        }writer.write("----------------------------------------end-----------------------------------------\n");
        writer.close();
    }
}



