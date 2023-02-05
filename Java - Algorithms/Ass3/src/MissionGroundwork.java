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
import java.util.*;

public class MissionGroundwork {



    public void printSchedule(List<Project> projectList) {
        for (int i = 0; i < projectList.size(); i++) {
            projectList.get(i).printSchedule(projectList.get(i).getEarliestSchedule());
        }
    }

    /**
     * TODO: Parse the input XML file and return a list of Project objects
     *
     * @param filename the input XML file
     * @return a list of Project objects
     */
    public List<Project> readXML(String filename) {
        List<Project> projectList = new ArrayList<>();


        try {
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            Document doc = db.parse(new File(filename));
            doc.getDocumentElement().normalize();
            NodeList projects = doc.getElementsByTagName("Project");

            for (int i = 0; i < projects.getLength(); i++) {

                Node project = projects.item(i);

                List<Task> tasks = new ArrayList<>();
                if (project.getNodeType() == Node.ELEMENT_NODE) {

                    Element element = (Element) project;
                    String name = element.getElementsByTagName("Name").item(0).getTextContent();
                    NodeList tasksList = element.getElementsByTagName("Task");
                    int counter = 0;
                    for (int j = 0; j < tasksList.getLength(); j++) {
                        if (tasksList.item(j).getNodeType() == Node.ELEMENT_NODE){
                            Element task = (Element) tasksList.item(j);
                            int Id = Integer.parseInt(task.getElementsByTagName("TaskID").item(0).getTextContent());
                            String Description = task.getElementsByTagName("Description").item(0).getTextContent();
                            int Duration = Integer.parseInt(task.getElementsByTagName("Duration").item(0).getTextContent());
                            counter += Duration;
                            NodeList DependenciesList = task.getElementsByTagName("DependsOnTaskID");
                            List<Integer> Dependencies = new ArrayList<>();
                            for (int k = 0; k < DependenciesList.getLength(); k++) {
                                if(DependenciesList.item(k).getNodeType() == Node.ELEMENT_NODE && DependenciesList.getLength() != 0){
                                    Element dependent = (Element) DependenciesList.item(k);
                                    int DependsOnTaskID = Integer.parseInt(DependenciesList.item(k).getTextContent());
                                    Dependencies.add(DependsOnTaskID);
                                }
                            }
                            Task taskObject = new Task(Id, Description, Duration, Dependencies);
                            tasks.add(taskObject);
                        }
                    }
                    Project Project = new Project(name, tasks);
                    Project.maxTime = counter;
                    projectList.add(Project);
                }
            }

        } catch (ParserConfigurationException | SAXException | IOException e) {
            e.printStackTrace();
        }
        return projectList;
    }

}
