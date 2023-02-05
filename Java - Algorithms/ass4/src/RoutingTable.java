import java.io.Serializable;
import java.sql.Struct;
import java.util.*;

public class RoutingTable implements Serializable {

    static final long serialVersionUID = 99L;
    private final Router router;
    private final Network network;
    private final List<RoutingTableEntry> entryList;

    public RoutingTable(Router router) {
        this.router = router;
        this.network = router.getNetwork();
        this.entryList = new ArrayList<>();
    }

    /**
     * updateTable() should calculate routing information and then instantiate RoutingTableEntry objects, and finally add
     * them to RoutingTable object’s entryList.
     */
    public void updateTable() {
        this.entryList.clear();
        for (Router destination  : network.getRouters()) {
            RoutingTableEntry entry  = new RoutingTableEntry(this.router.getIpAddress(), destination.getIpAddress(), new Stack<>());
            if(destination == this.router) entry.setTotalRouteCost(0.0);
            entryList.add(entry);
        }
        pathto(router);
        this.entryList.remove(getEntry_WithIP(router.getIpAddress()));
        for(RoutingTableEntry entry : entryList){
            if (!entry.getFullPath().isEmpty()) {
                entry.setNextRouterIpAddr(entry.getFullPath().firstElement().getOtherIpAddress(router.getIpAddress()));
            }
        }
    }

    /**
     * pathTo(Router destination) should return a Stack<Link> object which contains a stack of Link objects,
     * which represents a valid path from the owner Router to the destination Router.
     *
     * @param destination Destination router
     * @return Stack of links on the path to the destination router
     */

    public Stack<Link> pathTo(Router destination) {
        return new Stack<>();
    }

    public void pathto(Router destination){
        destination.setDown(true);
        List<Link> neigbours = network.getLinksOfRouter(destination);
        if (neigbours == null){
            return;
        }
        for (Link link : neigbours) {
            if(!getEntry_WithIP(destination.getIpAddress()).containsPath(link)){
                String other = link.getOtherIpAddress(destination.getIpAddress());
                double costFrom = getEntry_WithIP(destination.getIpAddress()).getTotalRouteCost();
                double costTo = getEntry_WithIP(other).getTotalRouteCost();
                if((costFrom + link.getCost()) < costTo) {
                    if(costTo != Double.POSITIVE_INFINITY){
                        network.getRouterWithIp(other).setDown(false);
                    }
                    getEntry_WithIP(other).setTotalRouteCost(costFrom + link.getCost());
                    getEntry_WithIP(other).setFullPath(new Stack<>());
                    for (Link l : getEntry_WithIP(destination.getIpAddress()).getFullPath()){
                        getEntry_WithIP(other).addLinkToPath(l);
                    }
                    getEntry_WithIP(other).addLinkToPath(link);
                }
            }
        }
        for (Link link : neigbours){
            String other = link.getOtherIpAddress(destination.getIpAddress());
            if((!getEntry_WithIP(destination.getIpAddress()).containsPath(link) && !network.getRouterWithIp(other).isDown())){
                pathto(network.getRouterWithIp(other));
            }
        }
    }

    public RoutingTableEntry getEntry_WithIP(String ip) {
        for (RoutingTableEntry entry : entryList) {
            if (entry.getDestinationIpAddr().equals(ip))
                return entry;
        }
        return null;
    }


    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        RoutingTable that = (RoutingTable) o;
        return router.equals(that.router) && entryList.equals(that.entryList);
    }

    public List<RoutingTableEntry> getEntryList() {
        return entryList;
    }
}
