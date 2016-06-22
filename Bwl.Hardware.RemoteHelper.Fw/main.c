#define BAUD 250000

#define DEV_NAME "BwlRemHelp1.0      "
#define ADC_ADJ ADC_ADJUST_RIGHT
#define ADC_REF ADC_REFS_INTERNAL_2_56
#define ADC_CLK ADC_PRESCALER_128

#include "board/board.h"
#include "board/movepointer_hal.h"
#include "refs-avr/bwl_ir.h"
#include "refs-avr/bwl_uart.h"
#include "refs-avr/bwl_simplserial.h"
#include "refs-avr/bwl_adc.h"
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
	//motor move
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
	//single adc avg
	if (sserial_request.command==10)
	{
		byte channel=		sserial_request.data[0];
		byte count=			sserial_request.data[1];
		adc_init_mux5(channel,ADC_ADJ,ADC_REF,ADC_CLK);
		int result=adc_read_average(count);
		
		sserial_response.result=128+sserial_request.command;
		sserial_response.datalength=2;
		sserial_response.data[0]=result>>8;
		sserial_response.data[1]=result&255;
		sserial_send_response();
	}	
	//single adc continue
	if (sserial_request.command==11)
	{
		int result=adc_read_once();
		
		sserial_response.result=128+sserial_request.command;
		sserial_response.datalength=2;
		sserial_response.data[0]=result>>8;
		sserial_response.data[1]=result&255;
		sserial_send_response();
	}
	//all adc
	if (sserial_request.command==12)
	{
		byte count=			sserial_request.data[1];
		adc_init_mux5(0,ADC_ADJ,ADC_REF,ADC_CLK);	int result0=adc_read_average(count);
		adc_init_mux5(1,ADC_ADJ,ADC_REF,ADC_CLK);	int result1=adc_read_average(count);
		adc_init_mux5(2,ADC_ADJ,ADC_REF,ADC_CLK);	int result2=adc_read_average(count);
		adc_init_mux5(3,ADC_ADJ,ADC_REF,ADC_CLK);	int result3=adc_read_average(count);
		adc_init_mux5(4,ADC_ADJ,ADC_REF,ADC_CLK);	int result4=adc_read_average(count);
		adc_init_mux5(5,ADC_ADJ,ADC_REF,ADC_CLK);	int result5=adc_read_average(count);
		adc_init_mux5(6,ADC_ADJ,ADC_REF,ADC_CLK);	int result6=adc_read_average(count);
		adc_init_mux5(7,ADC_ADJ,ADC_REF,ADC_CLK);	int result7=adc_read_average(count);
		
		sserial_response.result=128+sserial_request.command;
		sserial_response.datalength=16;
		sserial_response.data[0]=result0>>8;
		sserial_response.data[1]=result0&255;
		sserial_response.data[2]=result1>>8;
		sserial_response.data[3]=result1&255;
		sserial_response.data[4]=result2>>8;
		sserial_response.data[5]=result2&255;
		sserial_response.data[6]=result3>>8;
		sserial_response.data[7]=result3&255;
		sserial_response.data[8]=result4>>8;
		sserial_response.data[9]=result4&255;
		sserial_response.data[10]=result5>>8;
		sserial_response.data[11]=result5&255;
		sserial_response.data[12]=result6>>8;
		sserial_response.data[13]=result6&255;
		sserial_response.data[14]=result7>>8;
		sserial_response.data[15]=result7&255;		
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