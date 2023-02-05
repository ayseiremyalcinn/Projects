public class StuntPerformer extends Performer{
    private int height;
    private  int[] realId;

    public StuntPerformer(int id, String name, String surname, String country, int height, int[] realId) {
        super(id, name, surname, country);
        setHeight(height);
        setRealId(realId);
    }

    public int[] getRealId() {
        return realId;
    }

    public void setRealId(int[] realId) {
        this.realId = realId;
    }

    public int getHeight() {
        return height;
    }

    public void setHeight(int height) {
        this.height = height;
    }

}
