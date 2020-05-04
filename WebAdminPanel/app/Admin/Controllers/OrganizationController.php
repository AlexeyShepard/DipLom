<?php

namespace App\Admin\Controllers;

use App\Organization;
use Encore\Admin\Controllers\AdminController;
use Encore\Admin\Form;
use Encore\Admin\Grid;
use Encore\Admin\Show;

class OrganizationController extends CustomAdminController
{
    /**
     * Title for current resource.
     *
     * @var string
     */
    protected $title = 'Organizations';

    /**
     * Make a grid builder.
     *
     * @return Grid
     */
    protected function grid()
    {
        $grid = new Grid(new Organization());
        $grid->disableExport();

        $grid->filter(function($filter)
        {
            $filter->disableIdFilter();
            $filter->like('Name', __(trans('base.Name')));          
        });

        $grid->column('id', __('Id'));
        $grid->column('Name', __(trans('base.Name')));
        $grid->column(__(trans('base.Parent')))->display(function()
        {
            $organization = Organization::where('id', $this->Parent)->first();

            if($organization != null) return $organization->Name;
            else return "";
        });

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
        $show = new Show(Organization::findOrFail($id));

        $show->field('id', __('Id'));
        $show->field('Name', __(trans('base.Name')));
        $show->field(trans('base.Parent'))->as(function()
        {
            $organization = Organization::where('id', $this->Parent)->first();

            if($organization != null) return $organization->Name;
            else return "";
        });

        return $show;
    }

    /**
     * Make a form builder.
     *
     * @return Form
     */
    protected function form()
    {
        $form = new Form(new Organization());
        $form->text('Name', __(trans('base.Name')))->rules('required');

        $form->select('Parent', __(trans('base.Parent')))->options(function ($id) {
            $organization = Organization::all();

            $options = [];

            foreach($organization as $org) $options += [$org->id => $org->Name];
        
            return $options;
        });

        return $form;
    }
}
