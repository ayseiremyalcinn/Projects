public class TvSeries extends Film{
    private String startDate;
    private String endDate;
    private String[] genre;
    private int[] writers;
    private int season;
    private int episode;

    public TvSeries(int id, String title, String language, int[] directors, int length, String country, int[] performers, String[] genre, int[] writers, String startDate, String endDate, int season, int episode) {
        super(id, title, language, country, directors, length, performers);
        setStartDate(startDate);
        setEndDate(endDate);
        setGenre(genre);
        setWriters(writers);
        setSeason(season);
        setEpisode(episode);
    }

    public String getStartDate() {
        return startDate;
    }

    public String getEndDate() {
        return endDate;
    }

    public String[] getGenre() {
        return genre;
    }

    public int[] getWriters() {
        return writers;
    }

    public int getSeason() {
        return season;
    }

    public int getEpisode() {
        return episode;
    }

    public void setStartDate(String startDate) {
        this.startDate = startDate;
    }

    public void setEndDate(String endDate) {
        this.endDate = endDate;
    }

    public void setGenre(String[] genre) {
        this.genre = genre;
    }

    public void setWriters(int[] writers) {
        this.writers = writers;
    }

    public void setSeason(int season) {
        this.season = season;
    }

    public void setEpisode(int episode) {
        this.episode = episode;
    }
}
