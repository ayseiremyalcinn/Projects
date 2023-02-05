import java.io.*;
import java.util.ArrayList;

public class Article {

    public ArrayList<String> getFile(ArrayList<String> article_file ,String dosyaAdi) {
        try {
            BufferedReader br = new BufferedReader(new FileReader("src\\" + dosyaAdi));


            while (true) {
                String a = br.readLine();
                if (a != null) {
                    article_file.add(a);
                } else {
                    break;
                }
            }
            return article_file;
        }
        catch (IOException ex) {
            ex.printStackTrace();
        }

        return article_file;
    }
}
