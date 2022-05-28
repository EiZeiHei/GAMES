#include "viewwidget.h"

ViewWidget::ViewWidget(QWidget* parent)
	: QWidget(parent)
{
	ui.setupUi(this);
	draw_status_ = false;
	shape_ = NULL;
	type_ = Shape::kDefault;
}

ViewWidget::~ViewWidget()
{
	for (int i = 0; i < shape_list_.size(); i++)
	{
		if (shape_list_[i])
		 {
		 	delete shape_list_[i];
		 	shape_list_[i] = NULL;
		 }
	}
}

void ViewWidget::setLine()
{
	type_ = Shape::kLine;
}

void ViewWidget::setRect()
{
	type_ = Shape::kRect;
}

void ViewWidget::setEllipse()
{
	type_ = Shape::kEllipse;
}

void ViewWidget::setPolygon() {
    type_ = Shape::kPolygon;
}

void ViewWidget::setFreeHand() {
    type_ = Shape::kFreeHand;
}

void ViewWidget::Undo() {
    if (shape_list_.size() > 0) {
        delete shape_list_.back();
        shape_list_.pop_back();
    }
}

void ViewWidget::mousePressEvent(QMouseEvent* event)
{
	if (Qt::LeftButton == event->button())
	{
		switch (type_)
		{
        case Shape::kDefault:
            break;
		case Shape::kLine:
			shape_ = new Line();
			break;
		case Shape::kRect:
			shape_ = new Rect();
			break;
		case Shape::kEllipse:
			shape_ = new class Ellipse();
			break;
        case Shape::kPolygon:
            if (shape_ == NULL) {
                shape_ = new class Polygon();
                setMouseTracking(true);
            }
            break;
        case Shape::kFreeHand:
            shape_ = new FreeHand();
            break;
		}
		if (shape_ != NULL)
		{
			draw_status_ = true;
			start_point_ = end_point_ = event->pos();
			shape_->set_start(start_point_);
			shape_->set_end(end_point_);
		}
	}
	update();
}

void ViewWidget::mouseMoveEvent(QMouseEvent* event)
{
	if (draw_status_ && shape_ != NULL)
	{
		end_point_ = event->pos();
		shape_->set_end(end_point_);
	}
}

void ViewWidget::mouseReleaseEvent(QMouseEvent* event)
{
	if (shape_ != NULL)
	{
        if (type_ == Shape::kPolygon) {
            if (Qt::LeftButton == event->button())
                shape_->Update(1);
            else if (Qt::RightButton == event->button()) {
                shape_->Update(0);
                shape_list_.push_back(shape_);
                shape_ = NULL;
            }
        } else {
            draw_status_ = false;
            shape_list_.push_back(shape_);
            shape_ = NULL;
        }
    }
}

void ViewWidget::paintEvent(QPaintEvent*)
{
	QPainter painter(this);

	for (int i = 0; i < shape_list_.size(); i++)
	{
		shape_list_[i]->Draw(painter);
	}

	if (shape_ != NULL) {
		shape_->Draw(painter);
	}

	update();
}