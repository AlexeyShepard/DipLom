<?php

namespace App\Admin\Controllers;

use App\People;
use App\PeopleContact;
use App\Pincode;
use Encore\Admin\Controllers\AdminController;
use Encore\Admin\Form;
use Encore\Admin\Grid;
use Encore\Admin\Show;

class PeopleController extends AdminController
{
    /**
     * Title for current resource.
     *
     * @var string
     */
    protected $title = 'App\People';

    /**
     * Make a grid builder.
     *
     * @return Grid
     */
    protected function grid()
    {
        $grid = new Grid(new People());

        $grid->column('id', __('Id'));
        $grid->column('SurName', __('SurName'));
        $grid->column('FirstName', __('FirstName'));
        $grid->column('PatronymicName', __('PatronymicName'));
        $grid->column('Login', __('Login'));
        $grid->column('Password', __('Password'));
        $grid->column('DateTimeCreate', __('DateTimeCreate'));
        $grid->column('ActivityStatus', __('ActivityStatus'));
        $grid->column('Active Pincode')->display(function()
        {
          $active_pincode = Pincode::where('id_People', $this->id)->orderBy('id', 'desc')->first();

          if($active_pincode != null) return $active_pincode->PinCode;
          else return "Его не существует!";
        });

        $grid->column('Phonenumber')->display(function()
        {
            $actual_phone = PeopleContact::where('id_People', $this->id)->where('id_TypeContact', 1)->orderBy('id', 'desc')->first();

            if($actual_phone != null) return $actual_phone->Contact;
            else return "Его не существует!";
        });

        $grid->column('Email')->display(function()
        {
            $actual_email = PeopleContact::where('id_People', $this->id)->where('id_TypeContact', 2)->orderBy('id', 'desc')->first();

            if($actual_email != null) return $actual_email->Contact;
            else return "Его не существует!";
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
        $show = new Show(People::findOrFail($id));

        $show->field('id', __('Id'));
        $show->field('SurName', __('SurName'));
        $show->field('FirstName', __('FirstName'));
        $show->field('PatronymicName', __('PatronymicName'));
        $show->field('Login', __('Login'));
        $show->field('Password', __('Password'));
        $show->field('DateTimeCreate', __('DateTimeCreate'));
        $show->field('ActivityStatus', __('ActivityStatus'));

        return $show;
    }

    /**
     * Make a form builder.
     *
     * @return Form
     */
    protected function form()
    {
        $form = new Form(new People());

        $form->text('SurName', __('SurName'));
        $form->text('FirstName', __('FirstName'));
        $form->text('PatronymicName', __('PatronymicName'));
        $form->text('Login', __('Login'));
        $form->password('Password', __('Password'));
        $form->datetime('DateTimeCreate', __('DateTimeCreate'))->default(date('Y-m-d H:i:s'));
        $form->text('ActivityStatus', __('ActivityStatus'));

        return $form;
    }
}
