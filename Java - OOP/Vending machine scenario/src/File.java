import java.io.*;

import java.util.ArrayList;

public class File {

    public ArrayList<String> readFile(String input) throws IOException {
        BufferedReader br = new BufferedReader(new FileReader(input));
        ArrayList<String> file = new ArrayList<>();
        while (true) {
            String data = br.readLine();
            if (data != null) {
                file.add(data);
            } else {
                break;
            }
        }
        return file;
    }


}
