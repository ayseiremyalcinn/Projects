import java.io.FileNotFoundException;
import java.util.List;

public class TestAddRouter {
    public static void main(String[] args) {
        performTest(args);
    }

    static void performTest(String[] args) {
        try {
            List<RoutingTable> expectedRoutingTableList = (List<RoutingTable>) TestUtils.readDatObject(TestUtils.TEST_ADD_ROUTER_LINK_OUTPUT_FILE);
            double score = 0;
            TestUtils.disableOutput();

            Network network = new Network(args[0]);
            Router newRouter = new Router("192.168.0.33", network);
            network.addRouter(newRouter);
            network.addLink(new Link(newRouter.getIpAddress(), "192.168.0.41", 1000));
            network.addLink(new Link(newRouter.getIpAddress(), "192.168.0.8", 1000));
            network.addLink(new Link(newRouter.getIpAddress(), "192.168.1.13", 300));
            network.addLink(new Link(newRouter.getIpAddress(), "192.168.2.77", 300));

            Router anotherNewRouter = new Router("192.168.2.81", network);
            network.addRouter(anotherNewRouter);

            List<RoutingTable> routingTableListUUT = network.getRoutingTablesOfAllRouters();
            score = TestUtils.compareRoutingTables(expectedRoutingTableList, routingTableListUUT);

            TestUtils.enableOutput();
            System.out.println(score);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
    }
}
