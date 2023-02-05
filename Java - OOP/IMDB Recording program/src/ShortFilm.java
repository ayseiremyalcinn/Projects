public class ShortFilm extends Film{
    private String releaseDate;
    private int[] writers;
    private String[] genre;

    public ShortFilm(int id, String title, String language, int[] directors, int length, String country, int[] performers, String[] genre, String releaseDate, int[] writers) {
        super(id, title, language, country, directors, length, performers);
        setReleaseDate(releaseDate);
        setWriters(writers);
        setGenre(genre);
    }

    public String getReleaseDate() {
        return releaseDate;
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

    public void setWriters(int[] writers) {
        this.writers = writers;
    }

    public void setGenre(String[] genre) {
        this.genre = genre;
    }
}
