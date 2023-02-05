import java.io.FileNotFoundException;

public class TestReadNetwork {
    public static void main(String[] args) throws FileNotFoundException {
        TestUtils.disableOutput();
        Network expectedNetwork = (Network) TestUtils.readDatObject(TestUtils.TEST_READ_NETWORK_OUTPUT_FILE);
        Network networkUUT = new Network(args[0]);

        double correctRouters = 0;
        double correctLinks = 0;

        for (int i = 0; i < networkUUT.getRouters().size(); i++) {
            if (expectedNetwork.getRouters().get(i).equals(networkUUT.getRouters().get(i))) {
                correctRouters++;
            }
        }

        double routerPoints = correctRouters / expectedNetwork.getRouters().size();

        for (int i = 0; i < networkUUT.getLinks().size(); i++) {
            if (expectedNetwork.getLinks().get(i).equals(networkUUT.getLinks().get(i))) {
                correctLinks++;
            }
        }

        double linkPoints = correctLinks / expectedNetwork.getLinks().size();

        TestUtils.enableOutput();
        System.out.println(routerPoints * 0.7 + linkPoints * 0.3);
    }
}
