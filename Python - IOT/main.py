import pycom, socket
import time
import uos
import machine, os, network
from machine import SD
from machine import UART
from network import WLAN
import _thread
from machine import Pin

videoname = ""
videosize = ""
aktarim = False

uos.dupterm(None)

uart = UART(0, baudrate=115200)
uart.init(115200)


def sdWrite(str):
    try:
        if str != None:
            f = open(fileName,'a')
            if str.decode("utf-8").split(',')[1] == "1":
                f.write("\n---------------------------------------------------------UYDU YENIDEN BASLATILDI---------------------------------------------------------\n\n")
            f.write(str)
            f.write('\n')
            f.close()
    except:
        return



def checkvideo():
    global videoname
    global videosize
    global aktarim
    if videoname != "":
        try:
            stat = os.stat("/sd/" + videoname)
            size = stat[6]
            pycom.rgbled(0xff0000)
            if(size >= (int(videosize))):
                pycom.rgbled(0x0000ff)
                uart.write("V\n\r")
                aktarim = True
        except:
            return


def sendData():
    try:
        global aktarim
        while True:

            time.sleep(1)
            #pycom.rgbled(0xffffff)
            newData = uart.read()
            if newData != None:
                print(newData.decode("utf-8"))
            sdWrite(newData)
            if not aktarim:
                checkvideo()
    except:
        return

wlan = WLAN()
if machine.reset_cause() != machine.SOFT_RESET:
    wlan.init(mode=WLAN.STA)
    # configuration below MUST match your home router settings!!
    wlan.ifconfig(config=('192.168.1.35', '255.255.255.0', '192.168.1.10', '8.8.8.8')) # (ip, subnet_mask, gateway, DNS_server)

if not wlan.isconnected():
    # change the line below to match your network ssid, security and password
    wlan.connect('ubnt', auth=(WLAN.WPA2, 'teamcervos'), timeout=5000)

wlan.antenna(WLAN.EXT_ANT)

sd = SD()
os.mount(sd,'/sd')

newData0 = uart.read()
while newData0 == None:
    newData0 = uart.read()

saat = newData0.decode("utf-8").split(',')[2]
fileName = "/sd/" + saat.split(':')[0] + "."+ saat.split(':')[1] + "." + saat.split(':')[2] + "-Telemetri.txt"


f = open(fileName,'w')

_thread.start_new_thread(sendData, ())


pycom.heartbeat(0)
pycom.rgbled(0x0000ff)
