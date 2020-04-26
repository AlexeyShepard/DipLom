<?php

namespace App\Admin\Controllers;

use App\People;
use App\Http\Controllers\Controller;
use Encore\Admin\Layout\Content;
use Encore\Admin\Grid;

class TestController extends Controller
{
    public function index(Content $content)
    {
        return $content->body($this->grid());
    }

    public function grid()
    {
        $grid = new Grid(new People());

        $grid->model()->where('id', '=', 1);

        $grid->column('id', __('Id'));
        $grid->column('SurName', __('SurName'));
        $grid->column('FirstName', __('FirstName'));
        $grid->column('PatronymicName', __('PatronymicName'));
        $grid->column('Login', __('Login'));
        $grid->column('Password', __('Password'));
        $grid->column('DateTimeCreate', __('DateTimeCreate'));
        $grid->column('ActivityStatus', __('ActivityStatus'));

        return $grid;
    }
}
