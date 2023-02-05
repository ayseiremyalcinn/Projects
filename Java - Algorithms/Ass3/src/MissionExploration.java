import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class MissionExploration {

    /**
     * Given a Galaxy object, prints the solar systems within that galaxy.
     * Uses exploreSolarSystems() and printSolarSystems() methods of the Galaxy object.
     *
     * @param galaxy a Galaxy object
     */
    public void printSolarSystems(Galaxy galaxy) {
        List<SolarSystem> solarSystems = galaxy.exploreSolarSystems();
        galaxy.printSolarSystems(solarSystems);
    }

    /**
     * TODO: Parse the input XML file and return a list of Planet objects.
     *
     * @param filename the input XML file
     * @return a list of Planet objects
     */
    public Galaxy readXML(String filename) {
        List<Planet> planetList = new ArrayList<>();

        try {
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            Document doc = db.parse(new File(filename));
            doc.getDocumentElement().normalize();
            NodeList planets = doc.getElementsByTagName("Planet");
            for (int i = 0; i < planets.getLength(); i++){
                Node planet = planets.item(i);
                if (planet.getNodeType() == Node.ELEMENT_NODE){
                    Element planett = (Element) planet;
                    String ID = planett.getElementsByTagName("ID").item(0).getTextContent();
                    int TechnologyLevel = Integer.parseInt(planett.getElementsByTagName("TechnologyLevel").item(0).getTextContent());
                    NodeList Neighbors = planett.getElementsByTagName("PlanetID");
                    List<String> neighbors = new ArrayList<>();
                    for (int k = 0; k < Neighbors.getLength(); k++){
                        if (Neighbors.item(k).getNodeType() == Node.ELEMENT_NODE){
                            Element Neighbor = (Element) Neighbors.item(k);
                            neighbors.add(Neighbor.getTextContent());
                        }
                    }
                    Planet planetObject = new Planet(ID, TechnologyLevel, neighbors);
                    planetList.add(planetObject);
                }
            }

        } catch (Exception e) {
            e.printStackTrace();
        }

        return new Galaxy(planetList);
    }
}
