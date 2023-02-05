import java.util.ArrayList;

public class User extends Person{
    private ArrayList<String> rates = new ArrayList<>();

    public User(int id, String name, String surname, String country) {
        super(id, name, surname, country);
    }

    public void addRate(int filmId, int rate){
        rates.add(filmId + ":" + rate);
    }

    public void removeRate(int filmId){
        String tmp = null;
        for (String r : getRates()) {
            if(r.contains(String.valueOf(filmId))){
                tmp = r;
                break;
            }
        }
        getRates().remove(tmp);
    }

    public ArrayList<String> getRates() { return rates; }

}
