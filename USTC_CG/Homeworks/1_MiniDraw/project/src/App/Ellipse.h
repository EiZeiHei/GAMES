#pragma once

#include "Shape.h"

class Ellipse : public Shape {
public:
	Ellipse();
	~Ellipse();

public:
	void Draw(QPainter& painter);

};

