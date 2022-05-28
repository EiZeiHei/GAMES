//
// Created by LA on 2022/5/28.
//

#include "Polygon.h"

Polygon::Polygon() {
    finish = false;
    polygon.push_back(start);
}

Polygon::~Polygon() noexcept {}

void Polygon::Update(int mode) {
    switch (mode) {
        case 0:
            finish = true;
            break;
        case 1:
            if (polygon.size() > 0)
                polygon.back() = end;
            polygon.push_back(polygon.back());
            break;
        default:
            break;
    }
}

void Polygon::Draw(QPainter &painter) {
    if (finish)
        painter.drawPolygon(polygon);
    else
        painter.drawPolyline(polygon);
}