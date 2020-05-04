<?php

namespace App\Admin\Controllers;

use App\People;
use App\EventsLogs;
use Encore\Admin\Controllers\AdminController;
use Encore\Admin\Form;
use Encore\Admin\Grid;
use Encore\Admin\Show;

class NotificationController extends CustomAdminController
{
    /**
     * Title for current resource.
     *
     * @var string
     */
    protected $title = 'Notifications';

    /**
     * Make a grid builder.
     *
     * @return Grid
     */
    protected function grid()
    {
        $grid = new Grid(new EventsLogs());

        $grid->disableExport();
        $grid->filter(function($filter)
        {
            $filter->disableIdFilter();
            $filter->date('DateTimeEvent', __(trans('base.DateTimeEvent')));           
        });

        $grid->disableExport();
        $grid->disableCreation();
        $grid->option('show_actions', false);
        $grid->option('show_row_selector', false);

        $grid->model()->where('id_EventType', '=', 4);
        $grid->column(trans('base.SFP'))->display(function()
        {
            $current_people = People::where('id', $this->id_People)->first();

            return $current_people->SurName." ".$current_people->FirstName." ".$current_people->PatronymicName;
        });
        $grid->column('Comments', __(trans('base.Content')));
        $grid->column('DateTimeEvent', __(trans('base.DateTimeEvent')));

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
        $show = new Show(EventsLogs::findOrFail($id));

        $show->field('id', __('Id'));
        $show->field('id_People', __('Id People'));
        $show->field('id_EventType', __('Id EventType'));
        $show->field('id_EventResult', __('Id EventResult'));
        $show->field('Comments', __('Comments'));
        $show->field('HumanOrder', __('HumanOrder'));
        $show->field('DateTimeEvent', __('DateTimeEvent'));

        return $show;
    }

    /**
     * Make a form builder.
     *
     * @return Form
     */
    protected function form()
    {
        $form = new Form(new EventsLogs());

        $form->number('id_People', __('Id People'));
        $form->number('id_EventType', __('Id EventType'));
        $form->number('id_EventResult', __('Id EventResult'));
        $form->text('Comments', __('Comments'));
        $form->text('HumanOrder', __('HumanOrder'))->default('Нет');
        $form->datetime('DateTimeEvent', __('DateTimeEvent'))->default(date('Y-m-d H:i:s'));

        return $form;
    }
}
