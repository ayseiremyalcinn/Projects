public class Film {
    private int id;
    private String title;
    private String language;
    private String country;
    private int[] directors;
    private int length;
    private int[] performers;

    public Film(int id, String title, String language, String country, int[] directors, int length, int[] performers) {
        this.id = id;
        this.title = title;
        this.language = language;
        this.country = country;
        this.directors = directors;
        this.length = length;
        this.performers = performers;
    }

    public int getId() {
        return id;
    }

    public String getTitle() {
        return title;
    }

    public String getLanguage() {
        return language;
    }

    public String getCountry() {
        return country;
    }

    public int[] getDirectors() {
        return directors;
    }

    public int getLength() {
        return length;
    }

    public int[] getPerformers() {
        return performers;
    }

    public void setId(int id) {
        this.id = id;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public void setLanguage(String language) {
        this.language = language;
    }

    public void setCountry(String country) {
        this.country = country;
    }

    public void setDirectors(int[] directors) {
        this.directors = directors;
    }

    public void setLength(int length) {
        this.length = length;
    }

    public void setPerformers(int[] performers) {
        this.performers = performers;
    }
}
