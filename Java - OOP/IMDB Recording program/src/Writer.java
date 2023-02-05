public class Writer extends Artist{
    private String style;


    public Writer(int id, String name, String surname, String country, String style) {
        super(id, name, surname, country);
        setStyle(style);
    }

    public String getStyle() {
        return style;
    }

    public void setStyle(String style) {
        this.style = style;
    }

}
