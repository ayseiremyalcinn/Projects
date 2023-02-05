import pycom, socket
import time
import uos
import machine, network
import os
from machine import SD
from machine import UART
from network import WLAN
import _thread
from machine import Pin
from network import Server

pycom.rgbled(0xff0000)







server = Server(login=('micro', 'python'), timeout=300)



if server.isrunning():
    print("baglanti kuruldu")


antenPin = Pin('P12',mode = Pin.OUT)
antenPin.value(1)




pycom.rgbled(0xffffff)
