public class FeatureFilm extends Film{
    private String releaseDate;
    private int budget;
    private int[] writers;
    private String[] genre;

    public FeatureFilm(int id, String title, String language, int[] directors, int length, String country, int[] performers, String[] genre, String releaseDate, int[] writers, int budget) {
        super(id, title, language, country, directors, length, performers);
        setReleaseDate(releaseDate);
        setBudget(budget);
        setWriters(writers);
        setGenre(genre);
    }

    public String getReleaseDate() {
        return releaseDate;
    }

    public int getBudget() {
        return budget;
    }

    public int[] getWriters() {
        return writers;
    }

    public String[] getGenre() {
        return genre;
    }

    public void setReleaseDate(String releaseDate) {
        this.releaseDate = releaseDate;
    }

    public void setBudget(int budget) {
        this.budget = budget;
    }

    public void setWriters(int[] writers) {
        this.writers = writers;
    }

    public void setGenre(String[] genre) {
        this.genre = genre;
    }
}
