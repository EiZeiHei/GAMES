//
// Created by LA on 2022/5/28.
//

#ifndef MINIDRAW_POLYGON_H
#define MINIDRAW_POLYGON_H

#include "shape.h"

class Polygon : public Shape {
public:
    Polygon();

    ~Polygon();

public:
    void Draw(QPainter &painter);
    void Update(int mode);
private:
    QPolygon polygon;
    bool finish;
};


#endif //MINIDRAW_POLYGON_H
