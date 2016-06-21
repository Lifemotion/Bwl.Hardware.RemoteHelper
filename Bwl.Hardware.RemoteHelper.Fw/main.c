#define BAUD 9600

#define DEV_NAME "BwlRemHelp1.0      "

#include "board/board.h"
#include "board/movepointer_hal.h"
#include "refs-avr/bwl_ir.h"
#include "refs-avr/bwl_uart.h"
#include "refs-avr/bwl_simplserial.h"
#include "refs-avr/strings.h"

char ir_send_last[64]={};
#include <util/setbaud.h>

void sserial_send_start(unsigned char portindex){};//{if (portindex==UART_485)	{DDRB|=(1<<6);PORTB|=(1<<6);}}

void sserial_send_end(unsigned char portindex){};//{if (portindex==UART_485)	{DDRB|=(1<<6);PORTB&=(~(1<<6));}}

void sserial_process_base();

void sserial_process_request(unsigned char portindex)
{
	sserial_process_base();
}

void sserial_process_base()
{
	//read buttons
	if (sserial_request.command==1)
	{
		sserial_response.result=128+sserial_request.command;
		sserial_response.datalength=8;
		sserial_send_response();
	}
	//motormove
	if (sserial_request.command==5)
	{
		sserial_response.result=128+sserial_request.command;
		sserial_response.datalength=0;
		int dist1=(int)sserial_request.data[0]-127;
		int stopdist1=(int)sserial_request.data[1];
		int dist2=(int)sserial_request.data[2]-127;
		int stopdist2=(int)sserial_request.data[3];
		motor_horizontal_move(dist1*10,stopdist1);
		motor_vertical_move(dist2*10,stopdist2);
		sserial_send_response();
	}	
}

int main(void)
{
	wdt_enable(WDTO_8S);
	pointer_laser(1);
	uart_init_withdivider(UART_USB,UBRR_VALUE);
	sserial_find_bootloader();
	sserial_set_devname(DEV_NAME);
	sserial_append_devname(15,12,__DATE__);
	sserial_append_devname(27,8,__TIME__);
	while(1)
	{
		sserial_poll_uart(UART_USB);
		wdt_reset();
	}
}