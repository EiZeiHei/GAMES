#include "Line.h"

Line::Line()
{
}

Line::~Line()
{
}

void Line::Draw(QPainter& paint)
{
	paint.drawLine(start.x(), start.y(), end.x(), end.y());
}
