#include <conio.h>
#include <stdio.h>

int main()
{
	printf("Press any key to crash...");
	_getch();
  	volatile char *x = 0;
  	return *x;
}