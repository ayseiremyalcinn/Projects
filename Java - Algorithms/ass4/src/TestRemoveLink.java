import java.io.FileNotFoundException;
import java.util.List;

public class TestRemoveLink {
    public static void main(String[] args) {
        performTest(args);
    }


    static void performTest(String[] args) {
        try {
            List<RoutingTable> expectedRoutingTableList = (List<RoutingTable>) TestUtils.readDatObject(TestUtils.TEST_REMOVE_LINK_OUTPUT_FILE);
            double score = 0;
            TestUtils.disableOutput();

            Network network = new Network(args[0]);
            Link link1 = network.getLinkBetweenRouters("192.168.0.41", "192.168.0.24");
            Link link2 = network.getLinkBetweenRouters("192.168.2.9", "192.168.2.11");
            Link link3 = network.getLinkBetweenRouters("192.168.1.45", "192.168.2.9");

            if (link1 != null && link2 != null && link3 != null) {
                network.removeLink(link1);
                network.removeLink(link2);
                network.removeLink(link3);
                List<RoutingTable> routingTableListUUT = network.getRoutingTablesOfAllRouters();
                score = TestUtils.compareRoutingTables(expectedRoutingTableList, routingTableListUUT);
            }

            TestUtils.enableOutput();
            System.out.println(score);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
    }
}
