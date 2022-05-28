#pragma once

#include "Shape.h"

class Line : public Shape {
public:
	Line();
	~Line();

public:
	void Draw(QPainter &paint);
};
