import java.util.*;

public class Galaxy {

    private final List<Planet> planets;
    private List<SolarSystem> solarSystems;

    public Galaxy(List<Planet> planets) {
        this.planets = planets;
    }

    /**
     * Using the galaxy's list of Planet objects, explores all the solar systems in the galaxy.
     * Saves the result to the solarSystems instance variable and returns a shallow copy of it.
     *
     * @return List of SolarSystem objects.
     */
    public List<SolarSystem> exploreSolarSystems() {
        solarSystems = new ArrayList<>();
        for (int i = 0; i < planets.size(); i++) {
            boolean exist = false;
            Planet planet = planets.get(i);
            for (int k = 0; k < solarSystems.size(); k++) {
                if(solarSystems.get(k).hasPlanet(planet.getId())){
                    for (int j = 0; j < planet.getNeighbors().size(); j++) {
                        if(!solarSystems.get(k).hasPlanet(planet.getNeighbors().get(j))){     // listede olmayan komsuyu ekler
                            solarSystems.get(k).addPlanet(this.getPlanetsByID(planet.getNeighbors().get(j)));
                        }
                    }
                    exist = true;
                }
            }
            if(!exist){
                SolarSystem solarSystem = new SolarSystem();
                solarSystem.addPlanet(planet);
                int exist1 = -1;
                for (int j = 0; j < planet.getNeighbors().size(); j++) {
                    for (int k = 0; k < solarSystems.size(); k++) {
                        SolarSystem solar = solarSystems.get(k);
                        if(solar.hasPlanet(planet.getNeighbors().get(j))){
                            exist1 = k;
                            break;
                        }
                    }
                    if(!solarSystem.hasPlanet(planet.getNeighbors().get(j)))
                        solarSystem.addPlanet(this.getPlanetsByID(planet.getNeighbors().get(j)));
                }
                if(exist1 != -1){
                    for (int l = 0; l < solarSystem.getPlanets().size(); l++){
                        if(solarSystems.get(exist1).hasPlanet(solarSystem.getPlanets().get(l).getId()))
                            solarSystems.get(exist1).addPlanet(solarSystem.getPlanets().get(l));
                    }
                }else{
                    solarSystems.add(solarSystem);
                }

            }
        }
        return new ArrayList<>(solarSystems);
    }

    public List<SolarSystem> getSolarSystems() {
        return solarSystems;
    }

    // FOR TESTING
    public Planet getPlanetsByID(String planetId) {
        for (int i = 0; i < planets.size(); i++) {
            if(planets.get(i).getId().equals(planetId)){
                return planets.get(i);
            }
        }
        return null;
    }


    // FOR TESTING
    public int getSolarSystemIndexByPlanetID(String planetId) {
        for (int i = 0; i < solarSystems.size(); i++) {
            SolarSystem solarSystem = solarSystems.get(i);
            if (solarSystem.hasPlanet(planetId)) {
                return i;
            }
        }
        return -1;
    }

    public void printSolarSystems(List<SolarSystem> solarSystems) {
        System.out.printf("%d solar systems have been discovered.%n", solarSystems.size());
        for (int i = 0; i < solarSystems.size(); i++) {
            SolarSystem solarSystem = solarSystems.get(i);
            List<Planet> planets = new ArrayList<>(solarSystem.getPlanets());
            Collections.sort(planets);
            System.out.printf("Planets in Solar System %d: %s", i + 1, planets);
            System.out.println();
        }
    }
}
