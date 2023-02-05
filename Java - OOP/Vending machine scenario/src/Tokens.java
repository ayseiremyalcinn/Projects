public class Tokens implements Comparable<Tokens>{
    String id;
    String type;
    int number;

    public Tokens(String id, String type, int number) {
        this.id = id;
        this.type = type;
        this.number = number;
    }


    @Override
    public int compareTo(Tokens token) {
        if(this.number > token.number){
            return 1;
        }
        else if(this.number < token.number){
            return -1;
        }
        return 0;
    }


    @Override
    public String toString() {
        return "Tokens{" +
                "id='" + id + '\'' +
                ", type='" + type + '\'' +
                ", number=" + number +
                '}';
    }
}
