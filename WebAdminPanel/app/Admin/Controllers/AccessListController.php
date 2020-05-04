<?php

namespace App\Admin\Controllers;

use App\AccessList;
use Encore\Admin\Controllers\AdminController;
use Encore\Admin\Form;
use Encore\Admin\Grid;
use Encore\Admin\Show;

class AccessListController extends CustomAdminController
{
    /**
     * Title for current resource.
     *
     * @var string
     */
    protected $title = 'AccessList';

    /**
     * Make a grid builder.
     *
     * @return Grid
     */
    protected function grid()
    {
        $grid = new Grid(new AccessList());
        $grid->disableExport();
        $grid->disableFilter();

        $grid->column('id', __('Id'));
        $grid->column('Access', __(trans('base.Access')));
        $grid->column('Comment', __(trans('base.Content')));

        return $grid;
    }

    /**
     * Make a show builder.
     *
     * @param mixed $id
     * @return Show
     */
    protected function detail($id)
    {
        $show = new Show(AccessList::findOrFail($id));

        $show->field('id', __('Id'));
        $show->field('Access', __(trans('base.Access')));
        $show->field('Comment', __(trans('base.Content')));

        return $show;
    }

    /**
     * Make a form builder.
     *
     * @return Form
     */
    protected function form()
    {
        $form = new Form(new AccessList());

        $form->text('Access', __(trans('base.Access')))->rules('required');
        $form->text('Comment', __(trans('base.Content')))->rules('required');

        return $form;
    }
}
