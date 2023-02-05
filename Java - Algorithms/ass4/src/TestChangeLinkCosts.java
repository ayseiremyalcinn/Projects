import java.io.FileNotFoundException;
import java.util.List;

public class TestChangeLinkCosts {
    public static void main(String[] args) {
        performTest(args);
    }

    static void performTest(String[] args) {
        try {
            List<RoutingTable> expectedRoutingTableList = (List<RoutingTable>) TestUtils.readDatObject(TestUtils.TEST_CHANGE_LINK_COSTS_OUTPUT_FILE);
            double score = 0;
            TestUtils.disableOutput();

            Network network = new Network(args[0]);
            Link link1 = network.getLinkBetweenRouters("192.168.0.24", "192.168.2.77");
            Link link2 = network.getLinkBetweenRouters("192.168.1.45", "192.168.2.9");

            if (link1 != null && link2 != null) {
                network.changeLinkCost(link1, 20);
                link2.setBandwidthInMbps(100);
                network.updateAllRoutingTables();
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
