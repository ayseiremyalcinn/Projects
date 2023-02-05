import java.io.FileNotFoundException;
import java.util.List;

public class TestInitializeRoutingTables {
    public static void main(String[] args) {
        performTest(args);
    }

    static void performTest(String[] args) {
        try {
            List<RoutingTable> expectedRoutingTableList = (List<RoutingTable>) TestUtils.readDatObject(TestUtils.TEST_INITIALIZE_ROUTING_TABLES_OUTPUT_FILE);
            double score = 0;
            TestUtils.disableOutput();

            Network network = new Network(args[0]);
            List<RoutingTable> routingTableListUUT = network.getRoutingTablesOfAllRouters();
            score = TestUtils.compareRoutingTables(expectedRoutingTableList, routingTableListUUT);

            TestUtils.enableOutput();
            System.out.println(score);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
    }
}
