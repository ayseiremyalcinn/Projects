import java.io.IOException;

public class TestRunAll {
    public static void main(String[] args) throws IOException {
        TestAddRouter.main(args);
        TestChangeLinkCosts.main(args);
        TestChangeRouterState.main(args);
        TestInitializeRoutingTables.main(args);
        TestReadNetwork.main(args);
        TestRegex.main(args);
        TestRemoveLink.main(args);
        TestRemoveRouter.main(args);
    }
}
