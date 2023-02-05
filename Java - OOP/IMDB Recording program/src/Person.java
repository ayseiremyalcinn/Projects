public class Person {
    private int id;
    private String name;
    private String surname;
    private String country;

    public Person(int id, String name, String surname, String country) {
        setId(id);
        setName(name);
        setSurname(surname);
        setCountry(country);
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getSurname() {
        return surname;
    }

    public String getCountry() {
        return country;
    }

    public void setId(int id) {
        this.id = id;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setSurname(String surname) {
        this.surname = surname;
    }

    public void setCountry(String country) {
        this.country = country;
    }
}
