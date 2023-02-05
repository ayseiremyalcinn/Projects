import java.util.*;

public class SubspaceCommunicationNetwork {

    private List<SolarSystem> solarSystems;

    /**
     * Perform initializations regarding your implementation if necessary
     * @param solarSystems a list of SolarSystem objects
     */
    public SubspaceCommunicationNetwork(List<SolarSystem> solarSystems) {
        this.solarSystems = solarSystems;
    }

    /**
     * Using the solar systems of the network, generates a list of HyperChannel objects that constitute the minimum cost communication network.
     * @return A list HyperChannel objects that constitute the minimum cost communication network.
     */
    public List<HyperChannel> getMinimumCostCommunicationNetwork() {
        List<HyperChannel> minimumCostCommunicationNetwork = new ArrayList<>();
        Planet[] planetList = new Planet[solarSystems.size()];
        for (int i = 0; i < solarSystems.size(); i++) {
            Planet most = new Planet("", 0, null);
            for (int j = 0; j < solarSystems.get(i).getPlanets().size(); j++) {
                if(solarSystems.get(i).getPlanets().get(j).getTechnologyLevel() > most.getTechnologyLevel()){
                    most = solarSystems.get(i).getPlanets().get(j);
                }
            }
            planetList[i] = most;
        }
        List<HyperChannel> allChannel = new ArrayList<>();
        for (int i = 0; i < planetList.length; i++) {
            for (int j = 0; j < planetList.length; j++) {
                HyperChannel hyperChannel = new HyperChannel(planetList[i], planetList[j], Constants.SUBSPACE_COMMUNICATION_CONSTANT/((planetList[i].getTechnologyLevel() + planetList[j].getTechnologyLevel())/2));
                allChannel.add(hyperChannel);
            }
        }
        List<Planet> gezilen = new ArrayList<>();
        allChannel.sort(new SortByCost());
        for (int i = 0; i < allChannel.size(); i++) {
            if(!(gezilen.contains(allChannel.get(i).getTo()) && gezilen.contains(allChannel.get(i).getFrom()))){
                minimumCostCommunicationNetwork.add(allChannel.get(i));
                if(!gezilen.contains(allChannel.get(i).getTo())){
                    gezilen.add(allChannel.get(i).getTo());
                }
                if(!gezilen.contains(allChannel.get(i).getFrom())){
                    gezilen.add(allChannel.get(i).getFrom());
                }
            }
            if(minimumCostCommunicationNetwork.size() == planetList.length-1){
                break;
            }
        }
        return minimumCostCommunicationNetwork;
    }

    public void printMinimumCostCommunicationNetwork(List<HyperChannel> network) {
        double sum = 0;
        for (HyperChannel channel : network) {
            Planet[] planets = {channel.getFrom(), channel.getTo()};
            Arrays.sort(planets);
            System.out.printf("Hyperchannel between %s - %s with cost %f", planets[0], planets[1], channel.getWeight());
            System.out.println();
            sum += channel.getWeight();
        }
        System.out.printf("The total cost of the subspace communication network is %f.", sum);
        System.out.println();
    }
}
class SortByCost implements Comparator<HyperChannel> {

    public int compare(HyperChannel a, HyperChannel b)
    {
        return b.getWeight().compareTo(a.getWeight());
    }
}