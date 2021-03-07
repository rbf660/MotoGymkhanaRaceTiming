EESchema Schematic File Version 4
EELAYER 30 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title ""
Date ""
Rev ""
Comp ""
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L SparkFun-RF:XBEE JP1
U 1 1 601EE360
P 2450 6700
F 0 "JP1" H 2450 7460 45  0000 C CNN
F 1 "XBEE" H 2450 7376 45  0000 C CNN
F 2 "XBEE" H 2450 7300 20  0001 C CNN
F 3 "" H 2450 6700 50  0001 C CNN
F 4 "XXX-00000" H 2450 7281 60  0000 C CNN "Field4"
	1    2450 6700
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR0101
U 1 1 601EFD45
P 1750 7200
F 0 "#PWR0101" H 1750 6950 50  0001 C CNN
F 1 "GND" H 1755 7027 50  0000 C CNN
F 2 "" H 1750 7200 50  0001 C CNN
F 3 "" H 1750 7200 50  0001 C CNN
	1    1750 7200
	1    0    0    -1  
$EndComp
$Comp
L Connector_Generic:Conn_01x19 J2
U 1 1 601F4B48
P 2450 1250
F 0 "J2" V 2575 1246 50  0000 C CNN
F 1 "Conn_01x19" V 2666 1246 50  0000 C CNN
F 2 "Connector_PinSocket_2.54mm:PinSocket_1x19_P2.54mm_Vertical" H 2450 1250 50  0001 C CNN
F 3 "~" H 2450 1250 50  0001 C CNN
	1    2450 1250
	0    1    1    0   
$EndComp
$Comp
L Connector_Generic:Conn_01x19 J1
U 1 1 601F7BE1
P 2450 1800
F 0 "J1" V 2667 1796 50  0000 C CNN
F 1 "Conn_01x19" V 2576 1796 50  0000 C CNN
F 2 "Connector_PinSocket_2.54mm:PinSocket_1x19_P2.54mm_Vertical" H 2450 1800 50  0001 C CNN
F 3 "~" H 2450 1800 50  0001 C CNN
	1    2450 1800
	0    -1   -1   0   
$EndComp
$Comp
L Connector_Generic:Conn_01x06 J3
U 1 1 601FF8D6
P 650 1550
F 0 "J3" H 568 1025 50  0000 C CNN
F 1 "Conn_01x06" H 568 1116 50  0000 C CNN
F 2 "Connector_PinSocket_2.54mm:PinSocket_1x06_P2.54mm_Horizontal" H 650 1550 50  0001 C CNN
F 3 "~" H 650 1550 50  0001 C CNN
	1    650  1550
	-1   0    0    1   
$EndComp
$Comp
L Transistor_Array:ULN2003A U1
U 1 1 602574F9
P 2500 3200
F 0 "U1" H 2500 3867 50  0000 C CNN
F 1 "ULN2003A" H 2500 3776 50  0000 C CNN
F 2 "Package_DIP:DIP-16_W7.62mm_Socket_LongPads" H 2550 2650 50  0001 L CNN
F 3 "http://www.ti.com/lit/ds/symlink/uln2003a.pdf" H 2600 3000 50  0001 C CNN
	1    2500 3200
	0    1    1    0   
$EndComp
$Comp
L power:GND #PWR0102
U 1 1 60258D0D
P 1900 3200
F 0 "#PWR0102" H 1900 2950 50  0001 C CNN
F 1 "GND" H 1905 3027 50  0000 C CNN
F 2 "" H 1900 3200 50  0001 C CNN
F 3 "" H 1900 3200 50  0001 C CNN
	1    1900 3200
	0    1    1    0   
$EndComp
$Comp
L power:+3V3 #PWR0103
U 1 1 602829CE
P 3350 950
F 0 "#PWR0103" H 3350 800 50  0001 C CNN
F 1 "+3V3" H 3365 1123 50  0000 C CNN
F 2 "" H 3350 950 50  0001 C CNN
F 3 "" H 3350 950 50  0001 C CNN
	1    3350 950 
	1    0    0    -1  
$EndComp
$Comp
L power:+3V3 #PWR0104
U 1 1 60284C34
P 1750 6300
F 0 "#PWR0104" H 1750 6150 50  0001 C CNN
F 1 "+3V3" V 1765 6428 50  0000 L CNN
F 2 "" H 1750 6300 50  0001 C CNN
F 3 "" H 1750 6300 50  0001 C CNN
	1    1750 6300
	0    -1   -1   0   
$EndComp
$Comp
L power:+5V #PWR0105
U 1 1 6028568A
P 1550 900
F 0 "#PWR0105" H 1550 750 50  0001 C CNN
F 1 "+5V" H 1565 1073 50  0000 C CNN
F 2 "" H 1550 900 50  0001 C CNN
F 3 "" H 1550 900 50  0001 C CNN
	1    1550 900 
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR0106
U 1 1 602867FC
P 3350 2000
F 0 "#PWR0106" H 3350 1750 50  0001 C CNN
F 1 "GND" H 3355 1827 50  0000 C CNN
F 2 "" H 3350 2000 50  0001 C CNN
F 3 "" H 3350 2000 50  0001 C CNN
	1    3350 2000
	1    0    0    -1  
$EndComp
Text GLabel 1300 6400 0    50   Input ~ 0
TXD
Wire Wire Line
	1300 6400 1750 6400
Text GLabel 1000 1550 2    50   Input ~ 0
TXD
Wire Wire Line
	1000 1550 850  1550
Text GLabel 1000 1650 2    50   Input ~ 0
RXD
Wire Wire Line
	1000 1650 850  1650
Text GLabel 1300 6500 0    50   Input ~ 0
RXD
Wire Wire Line
	1300 6500 1750 6500
$Comp
L power:GND #PWR0107
U 1 1 60289D8A
P 850 1250
F 0 "#PWR0107" H 850 1000 50  0001 C CNN
F 1 "GND" V 855 1122 50  0000 R CNN
F 2 "" H 850 1250 50  0001 C CNN
F 3 "" H 850 1250 50  0001 C CNN
	1    850  1250
	0    -1   -1   0   
$EndComp
Text GLabel 2150 2150 3    50   Input ~ 0
RXD
Wire Wire Line
	2150 2150 2150 2000
Text GLabel 2450 2150 3    50   Input ~ 0
TXD
Wire Wire Line
	2450 2150 2450 2000
$Comp
L Device:R R1
U 1 1 6028B14E
P 1400 6800
F 0 "R1" V 1193 6800 50  0000 C CNN
F 1 "330R" V 1284 6800 50  0000 C CNN
F 2 "Resistor_THT:R_Axial_DIN0207_L6.3mm_D2.5mm_P10.16mm_Horizontal" V 1330 6800 50  0001 C CNN
F 3 "~" H 1400 6800 50  0001 C CNN
	1    1400 6800
	0    1    1    0   
$EndComp
Wire Wire Line
	1550 6800 1750 6800
$Comp
L Device:LED D1
U 1 1 6028DC62
P 950 6800
F 0 "D1" H 943 7017 50  0000 C CNN
F 1 "LED" H 943 6926 50  0000 C CNN
F 2 "LED_THT:LED_D3.0mm" H 950 6800 50  0001 C CNN
F 3 "~" H 950 6800 50  0001 C CNN
	1    950  6800
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR0108
U 1 1 6028EA62
P 600 6800
F 0 "#PWR0108" H 600 6550 50  0001 C CNN
F 1 "GND" H 605 6627 50  0000 C CNN
F 2 "" H 600 6800 50  0001 C CNN
F 3 "" H 600 6800 50  0001 C CNN
	1    600  6800
	1    0    0    -1  
$EndComp
Wire Wire Line
	600  6800 800  6800
Wire Wire Line
	1100 6800 1250 6800
$Comp
L Device:R R2
U 1 1 6028FF51
P 3400 6800
F 0 "R2" V 3193 6800 50  0000 C CNN
F 1 "330R" V 3284 6800 50  0000 C CNN
F 2 "Resistor_THT:R_Axial_DIN0207_L6.3mm_D2.5mm_P10.16mm_Horizontal" V 3330 6800 50  0001 C CNN
F 3 "~" H 3400 6800 50  0001 C CNN
	1    3400 6800
	0    1    1    0   
$EndComp
$Comp
L Device:LED D2
U 1 1 60290F49
P 3800 6800
F 0 "D2" H 3793 6545 50  0000 C CNN
F 1 "LED" H 3793 6636 50  0000 C CNN
F 2 "LED_THT:LED_D3.0mm" H 3800 6800 50  0001 C CNN
F 3 "~" H 3800 6800 50  0001 C CNN
	1    3800 6800
	-1   0    0    1   
$EndComp
$Comp
L power:GND #PWR0109
U 1 1 60291EF5
P 3950 6900
F 0 "#PWR0109" H 3950 6650 50  0001 C CNN
F 1 "GND" H 3955 6727 50  0000 C CNN
F 2 "" H 3950 6900 50  0001 C CNN
F 3 "" H 3950 6900 50  0001 C CNN
	1    3950 6900
	1    0    0    -1  
$EndComp
Wire Wire Line
	3150 6800 3250 6800
Wire Wire Line
	3550 6800 3650 6800
Wire Wire Line
	3950 6800 3950 6900
$Comp
L power:+5V #PWR0110
U 1 1 602DAF95
P 2900 3800
F 0 "#PWR0110" H 2900 3650 50  0001 C CNN
F 1 "+5V" H 2915 3973 50  0000 C CNN
F 2 "" H 2900 3800 50  0001 C CNN
F 3 "" H 2900 3800 50  0001 C CNN
	1    2900 3800
	-1   0    0    1   
$EndComp
Wire Wire Line
	2900 3800 2900 3600
Wire Wire Line
	2250 2000 2250 2650
Wire Wire Line
	2250 2650 2500 2650
Wire Wire Line
	2500 2650 2500 2800
Wire Wire Line
	2350 2000 2350 2550
Wire Wire Line
	2350 2550 2600 2550
Wire Wire Line
	2600 2550 2600 2800
Wire Wire Line
	2550 2000 2550 2450
Wire Wire Line
	2550 2450 2700 2450
Wire Wire Line
	2700 2450 2700 2800
$Comp
L Relay:G5V-1 K1
U 1 1 602DD50C
P 1250 4000
F 0 "K1" V 683 4000 50  0000 C CNN
F 1 "G5V-1" V 774 4000 50  0000 C CNN
F 2 "RiderDetection:Relay_SPDT_Omron_G5V-1" H 2380 3970 50  0001 C CNN
F 3 "http://omronfs.omron.com/en_US/ecb/products/pdf/en-g5v_1.pdf" H 1250 4000 50  0001 C CNN
	1    1250 4000
	0    -1   -1   0   
$EndComp
$Comp
L Relay:G5V-1 K3
U 1 1 602E9A6F
P 3900 3950
F 0 "K3" V 4467 3950 50  0000 C CNN
F 1 "G5V-1" V 4376 3950 50  0000 C CNN
F 2 "RiderDetection:Relay_SPDT_Omron_G5V-1" H 5030 3920 50  0001 C CNN
F 3 "http://omronfs.omron.com/en_US/ecb/products/pdf/en-g5v_1.pdf" H 3900 3950 50  0001 C CNN
	1    3900 3950
	0    1    1    0   
$EndComp
$Comp
L Relay:G5V-1 K2
U 1 1 602EE852
P 3700 5250
F 0 "K2" V 3133 5250 50  0000 C CNN
F 1 "G5V-1" V 3224 5250 50  0000 C CNN
F 2 "RiderDetection:Relay_SPDT_Omron_G5V-1" H 4830 5220 50  0001 C CNN
F 3 "http://omronfs.omron.com/en_US/ecb/products/pdf/en-g5v_1.pdf" H 3700 5250 50  0001 C CNN
	1    3700 5250
	0    1    1    0   
$EndComp
Wire Wire Line
	2600 3600 2600 5050
Wire Wire Line
	2600 5050 3400 5050
$Comp
L power:+5V #PWR0111
U 1 1 602F4D4B
P 4300 3750
F 0 "#PWR0111" H 4300 3600 50  0001 C CNN
F 1 "+5V" V 4315 3878 50  0000 L CNN
F 2 "" H 4300 3750 50  0001 C CNN
F 3 "" H 4300 3750 50  0001 C CNN
	1    4300 3750
	0    1    1    0   
$EndComp
Wire Wire Line
	4300 3750 4200 3750
$Comp
L power:+5V #PWR0112
U 1 1 602F645C
P 800 4200
F 0 "#PWR0112" H 800 4050 50  0001 C CNN
F 1 "+5V" V 815 4328 50  0000 L CNN
F 2 "" H 800 4200 50  0001 C CNN
F 3 "" H 800 4200 50  0001 C CNN
	1    800  4200
	0    -1   -1   0   
$EndComp
Wire Wire Line
	950  4200 800  4200
$Comp
L power:+5V #PWR0113
U 1 1 602F7753
P 4150 5050
F 0 "#PWR0113" H 4150 4900 50  0001 C CNN
F 1 "+5V" V 4165 5178 50  0000 L CNN
F 2 "" H 4150 5050 50  0001 C CNN
F 3 "" H 4150 5050 50  0001 C CNN
	1    4150 5050
	0    1    1    0   
$EndComp
Wire Wire Line
	4000 5050 4150 5050
$Comp
L Connector:Screw_Terminal_01x02 PWR0
U 1 1 602FB922
P 4000 1200
F 0 "PWR0" V 3964 1012 50  0000 R CNN
F 1 "Screw_Terminal_01x02" V 3873 1012 50  0000 R CNN
F 2 "Connector_Phoenix_MSTB:PhoenixContact_MSTBA_2,5_2-G_1x02_P5.00mm_Horizontal" H 4000 1200 50  0001 C CNN
F 3 "~" H 4000 1200 50  0001 C CNN
	1    4000 1200
	0    -1   -1   0   
$EndComp
$Comp
L power:+12V #PWR0114
U 1 1 602FD6C0
P 4000 1500
F 0 "#PWR0114" H 4000 1350 50  0001 C CNN
F 1 "+12V" H 4015 1673 50  0000 C CNN
F 2 "" H 4000 1500 50  0001 C CNN
F 3 "" H 4000 1500 50  0001 C CNN
	1    4000 1500
	-1   0    0    1   
$EndComp
$Comp
L power:GND #PWR0115
U 1 1 602FE4F3
P 4150 1550
F 0 "#PWR0115" H 4150 1300 50  0001 C CNN
F 1 "GND" H 4155 1377 50  0000 C CNN
F 2 "" H 4150 1550 50  0001 C CNN
F 3 "" H 4150 1550 50  0001 C CNN
	1    4150 1550
	1    0    0    -1  
$EndComp
Wire Wire Line
	4000 1400 4000 1450
Wire Wire Line
	4100 1400 4100 1450
Wire Wire Line
	4100 1550 4150 1550
$Comp
L Connector:Screw_Terminal_01x04 STARTLIGHT0
U 1 1 60300486
P 5450 1200
F 0 "STARTLIGHT0" V 5414 912 50  0000 R CNN
F 1 "Screw_Terminal_01x04" V 5323 912 50  0000 R CNN
F 2 "Connector_Phoenix_MSTB:PhoenixContact_MSTBA_2,5_4-G_1x04_P5.00mm_Horizontal" H 5450 1200 50  0001 C CNN
F 3 "~" H 5450 1200 50  0001 C CNN
	1    5450 1200
	0    -1   -1   0   
$EndComp
Text GLabel 3500 4150 0    50   Input ~ 0
START_COLOR_COM
Wire Wire Line
	3600 4150 3500 4150
Text GLabel 1700 3800 2    50   Input ~ 0
START_COLOR_COM
Text GLabel 3250 5450 0    50   Input ~ 0
START_COLOR_COM
Wire Wire Line
	3250 5450 3400 5450
Text GLabel 4350 4350 3    50   Input ~ 0
COL_GRN
Wire Wire Line
	4350 4350 4350 4250
Wire Wire Line
	4350 4250 4200 4250
Text GLabel 800  3700 0    50   Input ~ 0
COL_RD
Wire Wire Line
	950  3700 800  3700
Text GLabel 4150 5550 2    50   Input ~ 0
COL_YW
Wire Wire Line
	4000 5550 4150 5550
Text GLabel 5650 1600 3    50   Input ~ 0
COL_RD
Wire Wire Line
	5650 1600 5650 1400
Text GLabel 5550 1600 3    50   Input ~ 0
COL_YW
Wire Wire Line
	5550 1600 5550 1400
Text GLabel 5450 1600 3    50   Input ~ 0
COL_GRN
Wire Wire Line
	5450 1600 5450 1400
Text GLabel 5350 1600 3    50   Input ~ 0
START_COM
Wire Wire Line
	5350 1600 5350 1400
$Comp
L Connector:Conn_01x03_Male J5
U 1 1 6030DD57
P 4650 2450
F 0 "J5" V 4712 2594 50  0000 L CNN
F 1 "Conn_01x03_Male" V 4803 2594 50  0000 L CNN
F 2 "Connector_PinHeader_2.54mm:PinHeader_1x03_P2.54mm_Vertical" H 4650 2450 50  0001 C CNN
F 3 "~" H 4650 2450 50  0001 C CNN
	1    4650 2450
	0    1    1    0   
$EndComp
$Comp
L power:GND #PWR0116
U 1 1 6030FEAC
P 4550 2850
F 0 "#PWR0116" H 4550 2600 50  0001 C CNN
F 1 "GND" H 4555 2677 50  0000 C CNN
F 2 "" H 4550 2850 50  0001 C CNN
F 3 "" H 4550 2850 50  0001 C CNN
	1    4550 2850
	1    0    0    -1  
$EndComp
Wire Wire Line
	4550 2850 4550 2650
$Comp
L power:+12V #PWR0117
U 1 1 6031180E
P 4750 2850
F 0 "#PWR0117" H 4750 2700 50  0001 C CNN
F 1 "+12V" H 4765 3023 50  0000 C CNN
F 2 "" H 4750 2850 50  0001 C CNN
F 3 "" H 4750 2850 50  0001 C CNN
	1    4750 2850
	-1   0    0    1   
$EndComp
Wire Wire Line
	4750 2850 4750 2650
Text GLabel 4650 3100 3    50   Input ~ 0
START_COLOR_COM
Wire Wire Line
	4650 3100 4650 2650
$Comp
L Connector:Conn_01x03_Male J4
U 1 1 603166F8
P 4200 2150
F 0 "J4" V 4262 2294 50  0000 L CNN
F 1 "Conn_01x03_Male" V 4353 2294 50  0000 L CNN
F 2 "Connector_PinHeader_2.54mm:PinHeader_1x03_P2.54mm_Vertical" H 4200 2150 50  0001 C CNN
F 3 "~" H 4200 2150 50  0001 C CNN
	1    4200 2150
	0    1    1    0   
$EndComp
$Comp
L power:GND #PWR0118
U 1 1 603166FE
P 4100 2550
F 0 "#PWR0118" H 4100 2300 50  0001 C CNN
F 1 "GND" H 4105 2377 50  0000 C CNN
F 2 "" H 4100 2550 50  0001 C CNN
F 3 "" H 4100 2550 50  0001 C CNN
	1    4100 2550
	1    0    0    -1  
$EndComp
Wire Wire Line
	4100 2550 4100 2350
$Comp
L power:+12V #PWR0119
U 1 1 60316705
P 4300 2550
F 0 "#PWR0119" H 4300 2400 50  0001 C CNN
F 1 "+12V" H 4315 2723 50  0000 C CNN
F 2 "" H 4300 2550 50  0001 C CNN
F 3 "" H 4300 2550 50  0001 C CNN
	1    4300 2550
	-1   0    0    1   
$EndComp
Wire Wire Line
	4300 2550 4300 2350
Text GLabel 4200 2800 3    50   Input ~ 0
START_COM
Wire Wire Line
	4200 2800 4200 2350
NoConn ~ 2100 3600
NoConn ~ 2200 3600
NoConn ~ 2300 3600
NoConn ~ 2400 3600
NoConn ~ 2400 2800
NoConn ~ 2300 2800
NoConn ~ 2200 2800
NoConn ~ 2100 2800
NoConn ~ 4200 4050
NoConn ~ 950  3900
NoConn ~ 4000 5350
NoConn ~ 3150 6300
NoConn ~ 3150 6400
NoConn ~ 3150 6500
NoConn ~ 3150 6600
NoConn ~ 3150 6700
NoConn ~ 1750 6600
NoConn ~ 1750 6700
NoConn ~ 3150 6900
NoConn ~ 3150 7000
NoConn ~ 3150 7100
NoConn ~ 3150 7200
NoConn ~ 1750 6900
NoConn ~ 1750 7000
NoConn ~ 1750 7100
NoConn ~ 850  1350
NoConn ~ 850  1450
NoConn ~ 850  1750
NoConn ~ 1650 1050
NoConn ~ 1750 1050
NoConn ~ 1850 1050
NoConn ~ 1950 1050
NoConn ~ 2050 1050
NoConn ~ 2150 1050
NoConn ~ 2250 1050
NoConn ~ 2350 1050
NoConn ~ 2450 1050
NoConn ~ 2550 1050
NoConn ~ 2650 1050
NoConn ~ 2750 1050
NoConn ~ 2850 1050
NoConn ~ 2950 1050
NoConn ~ 3050 1050
NoConn ~ 3150 1050
NoConn ~ 3250 1050
NoConn ~ 3250 2000
NoConn ~ 3150 2000
NoConn ~ 3050 2000
NoConn ~ 2950 2000
NoConn ~ 2850 2000
NoConn ~ 2750 2000
NoConn ~ 2650 2000
NoConn ~ 2050 2000
NoConn ~ 1950 2000
NoConn ~ 1850 2000
NoConn ~ 1750 2000
NoConn ~ 1650 2000
NoConn ~ 1550 2000
$Comp
L power:PWR_FLAG #FLG0101
U 1 1 603473B8
P 4000 1450
F 0 "#FLG0101" H 4000 1525 50  0001 C CNN
F 1 "PWR_FLAG" V 4000 1577 50  0000 L CNN
F 2 "" H 4000 1450 50  0001 C CNN
F 3 "~" H 4000 1450 50  0001 C CNN
	1    4000 1450
	0    -1   -1   0   
$EndComp
Connection ~ 4000 1450
Wire Wire Line
	4000 1450 4000 1500
Wire Wire Line
	3350 950  3350 1000
$Comp
L power:PWR_FLAG #FLG0102
U 1 1 6034C21E
P 3350 1000
F 0 "#FLG0102" H 3350 1075 50  0001 C CNN
F 1 "PWR_FLAG" V 3350 1128 50  0000 L CNN
F 2 "" H 3350 1000 50  0001 C CNN
F 3 "~" H 3350 1000 50  0001 C CNN
	1    3350 1000
	0    1    1    0   
$EndComp
Connection ~ 3350 1000
Wire Wire Line
	3350 1000 3350 1050
$Comp
L power:PWR_FLAG #FLG0104
U 1 1 6035206C
P 4100 1450
F 0 "#FLG0104" H 4100 1525 50  0001 C CNN
F 1 "PWR_FLAG" V 4100 1578 50  0000 L CNN
F 2 "" H 4100 1450 50  0001 C CNN
F 3 "~" H 4100 1450 50  0001 C CNN
	1    4100 1450
	0    1    1    0   
$EndComp
Connection ~ 4100 1450
Wire Wire Line
	4100 1450 4100 1550
$Comp
L Regulator_Switching:TSR_1-2450 U2
U 1 1 603634C2
P 1250 2600
F 0 "U2" V 1296 2830 50  0000 L CNN
F 1 "TSR_1-2450" V 1205 2830 50  0000 L CNN
F 2 "Converter_DCDC:Converter_DCDC_TRACO_TSR-1_THT" H 1250 2450 50  0001 L CIN
F 3 "http://www.tracopower.com/products/tsr1.pdf" H 1250 2600 50  0001 C CNN
	1    1250 2600
	0    -1   -1   0   
$EndComp
$Comp
L power:+12V #PWR0120
U 1 1 60364C65
P 1150 3150
F 0 "#PWR0120" H 1150 3000 50  0001 C CNN
F 1 "+12V" H 1165 3323 50  0000 C CNN
F 2 "" H 1150 3150 50  0001 C CNN
F 3 "" H 1150 3150 50  0001 C CNN
	1    1150 3150
	-1   0    0    1   
$EndComp
$Comp
L power:GND #PWR0121
U 1 1 603658AC
P 1550 2600
F 0 "#PWR0121" H 1550 2350 50  0001 C CNN
F 1 "GND" V 1555 2472 50  0000 R CNN
F 2 "" H 1550 2600 50  0001 C CNN
F 3 "" H 1550 2600 50  0001 C CNN
	1    1550 2600
	0    -1   -1   0   
$EndComp
$Comp
L power:+5V #PWR0122
U 1 1 603665CD
P 1150 2050
F 0 "#PWR0122" H 1150 1900 50  0001 C CNN
F 1 "+5V" H 1165 2223 50  0000 C CNN
F 2 "" H 1150 2050 50  0001 C CNN
F 3 "" H 1150 2050 50  0001 C CNN
	1    1150 2050
	1    0    0    -1  
$EndComp
Wire Wire Line
	1150 2050 1150 2200
Wire Wire Line
	1450 2600 1550 2600
Wire Wire Line
	1150 3000 1150 3150
Wire Wire Line
	1550 900  1550 1050
Wire Wire Line
	1550 3800 1700 3800
Wire Wire Line
	1550 4200 2500 4200
Wire Wire Line
	2500 4200 2500 3600
Wire Wire Line
	2700 3600 2700 4050
Wire Wire Line
	2700 4050 3600 4050
Wire Wire Line
	3600 4050 3600 3750
$Comp
L Mechanical:MountingHole H1
U 1 1 603B5B08
P 5700 2500
F 0 "H1" H 5800 2546 50  0000 L CNN
F 1 "MountingHole" H 5800 2455 50  0000 L CNN
F 2 "MountingHole:MountingHole_3.2mm_M3" H 5700 2500 50  0001 C CNN
F 3 "~" H 5700 2500 50  0001 C CNN
	1    5700 2500
	1    0    0    -1  
$EndComp
$Comp
L Mechanical:MountingHole H2
U 1 1 603B626E
P 5700 2700
F 0 "H2" H 5800 2746 50  0000 L CNN
F 1 "MountingHole" H 5800 2655 50  0000 L CNN
F 2 "MountingHole:MountingHole_3.2mm_M3" H 5700 2700 50  0001 C CNN
F 3 "~" H 5700 2700 50  0001 C CNN
	1    5700 2700
	1    0    0    -1  
$EndComp
$Comp
L Mechanical:MountingHole_Pad H3
U 1 1 603B717D
P 5700 2950
F 0 "H3" H 5800 2999 50  0000 L CNN
F 1 "MountingHole_Pad" H 5800 2908 50  0000 L CNN
F 2 "MountingHole:MountingHole_3.2mm_M3_Pad_Via" H 5700 2950 50  0001 C CNN
F 3 "~" H 5700 2950 50  0001 C CNN
	1    5700 2950
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR0123
U 1 1 603B7760
P 5700 3200
F 0 "#PWR0123" H 5700 2950 50  0001 C CNN
F 1 "GND" H 5705 3027 50  0000 C CNN
F 2 "" H 5700 3200 50  0001 C CNN
F 3 "" H 5700 3200 50  0001 C CNN
	1    5700 3200
	1    0    0    -1  
$EndComp
Wire Wire Line
	5700 3200 5700 3050
$Comp
L Mechanical:MountingHole H4
U 1 1 603C7FFB
P 5700 2300
F 0 "H4" H 5800 2346 50  0000 L CNN
F 1 "MountingHole" H 5800 2255 50  0000 L CNN
F 2 "MountingHole:MountingHole_3.2mm_M3" H 5700 2300 50  0001 C CNN
F 3 "~" H 5700 2300 50  0001 C CNN
	1    5700 2300
	1    0    0    -1  
$EndComp
$EndSCHEMATC
