#pragma once

#include <QtGui>

class Shape
{
public:
	Shape();
	virtual ~Shape();
	virtual void Draw(QPainter& paint) = 0;
    virtual void Update(int mode) {};
	void set_start(QPoint s);
	void set_end(QPoint e);

public:
	enum Type
	{
		kDefault = 0,
		kLine = 1,
		kRect = 2,
		kEllipse = 3,
        kPolygon = 4,
        kFreeHand = 5,
	};

protected:
	QPoint start;
	QPoint end;
};

