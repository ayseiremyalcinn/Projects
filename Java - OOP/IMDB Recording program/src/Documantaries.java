public class Documantaries extends Film{
    private String releaseDate;

    public Documantaries(int id, String title, String language, int[] directors, int length, String country, int[] performers, String releaseDate) {
        super(id, title, language, country, directors, length, performers);
        setReleaseDate(releaseDate);
    }

    public String getReleaseDate() {
        return releaseDate;
    }

    public void setReleaseDate(String releaseDate) {
        this.releaseDate = releaseDate;
    }
}
