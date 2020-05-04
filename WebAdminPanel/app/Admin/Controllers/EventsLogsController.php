<?php

namespace App\Admin\Controllers;

use App\EventsLogs;
use App\People;
use App\EventResult;
use Encore\Admin\Controllers\AdminController;
use Encore\Admin\Form;
use Encore\Admin\Grid;
use Encore\Admin\Show;

class EventsLogsController extends CustomAdminController
{
    /**
     * Title for current resource.
     *
     * @var string
     */
    protected $title = 'EventsLogs';

    /**
     * Make a grid builder.
     *
     * @return Grid
     */
    protected function grid()
    {
        $grid = new Grid(new EventsLogs());

        $grid->filter(function($filter)
        {
            $filter->disableIdFilter();
            $filter->date('DateTimeEvent', __(trans('base.DateTimeEvent')));
        });

        $grid->disableExport();
        $grid->disableCreation();
        $grid->option('show_actions', false);
        $grid->option('show_row_selector', false);

        $grid->column(trans('base.SFP'))->display(function()
        {
            $current_people = People::where('id', $this->id_People)->first();

            return $current_people->SurName." ".$current_people->FirstName." ".$current_people->PatronymicName;
        });
        $grid->column(trans('base.Result'))->display(function()
        {
            $event_result = EventResult::where('id', $this->id_EventResult)->first();

            return trans('base.' . $event_result->ResultName);
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
        $show->field('id_EventResult', __('Id EventResult'));
        $show->field('Comments', __('Comments'));
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
        $form->number('id_EventResult', __('Id EventResult'));
        $form->text('Comments', __('Comments'));
        $form->datetime('DateTimeEvent', __('DateTimeEvent'))->default(date('Y-m-d H:i:s'));

        return $form;
    }
}
