<?php

namespace App\Admin\Controllers;

use App\People;
use App\PeopleContact;
use App\Pincode;
use App\PeopleAccessInterim;
use App\AccessList;
use App\Organization;
use App\PeopleOrganizations;
use App\Admin\AccessGrid;
use App\Admin\OrganizationGrid;
use Encore\Admin\Form;
use Encore\Admin\Grid;
use Encore\Admin\Show;
use Encore\Admin\Layout\Content;

class PeopleController extends CustomAdminController
{

  /**
     * Title for current resource.
     *
     * @var string
     */
    protected $title = "Peoples";
  
    /**
     * Make a grid builder.
     *
     * @return Grid
     */
    protected function grid()
    {
           
        $grid = new Grid(new People());
        $grid->disableExport();
        $grid->filter(function($filter){
            $filter->disableIdFilter();
            $filter->like('SurName',__(trans('base.SurName')));
            $filter->like('FirstName', __(trans('base.FirstName')));
            $filter->like('PatronymicName', __(trans('base.PatronymicName')));
            $filter->like('Login', __(trans('base.Login')));
            $filter->date('DateTimeCreate', __(trans('base.DateTimeCreate')));

        });

        $grid->column('id', __('Id'));
        $grid->column('SurName', __(trans('base.SurName')));
        $grid->column('FirstName', __(trans('base.FirstName')));
        $grid->column('PatronymicName', __(trans('base.PatronymicName')));
        $grid->column('Login', __(trans('base.Login')));
        $grid->column('Password', __(trans('base.Password')));
        $grid->column('DateTimeCreate', __(trans('base.DateTimeCreate')));
        $grid->column('ActivityStatus', __(trans('base.ActivityStatus')));
        $grid->column(trans('base.ActualPincode'))->display(function()
        {
          $active_pincode = Pincode::where('id_People', $this->id)->orderBy('id', 'desc')->first();

          if($active_pincode != null) return $active_pincode->PinCode;
          else return "";
        });
        
        $grid->column(trans('base.PinCodeGenerationTime'))->display(function()
        {
            $active_pincode = Pincode::where('id_People', $this->id)->orderBy('id', 'desc')->first();

            if($active_pincode != null) return $active_pincode->DateTimeGen;
          else return "";
        });

        $grid->column(trans('base.PinCodeEndTime'))->display(function()
        {
            $active_pincode = Pincode::where('id_People', $this->id)->orderBy('id', 'desc')->first();

            if($active_pincode != null) return $active_pincode->EndTime;
          else return "";
        });

        $grid->column(trans('base.Phonenumber'))->display(function()
        {
            $actual_phone = PeopleContact::where('id_People', $this->id)->where('id_TypeContact', 1)->orderBy('id', 'desc')->first();

            if($actual_phone != null) return $actual_phone->Contact;
            else return "";
        });

        $grid->column(trans('base.Email'))->display(function()
        {
            $actual_email = PeopleContact::where('id_People', $this->id)->where('id_TypeContact', 2)->orderBy('id', 'desc')->first();

            if($actual_email != null) return $actual_email->Contact;
            else return "";
        });

        $grid->column(trans('base.Organization'))->display(function()
        {

          $organizations = PeopleOrganizations::where('id_People', $this->id)->get();

          $result = "";

          foreach($organizations as $org)
          {
            $OrgInfo = Organization::where('id', $org->id_Organizations)->first();

            $result .= $OrgInfo->Name . "\n";
          }
          
          return $result;
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
        
        return $show->render() . $this->renderAccessList(true, $id)->render() . $this->renderOrganizationList(true, $id)->render();
    }

    /**
     * Make a form builder.
     *
     * @return Form
     */
    protected function form()
    {
        $form = new Form(new People());

        $form->text('SurName', __(trans('base.SurName')))->rules('required|min:2');
        $form->text('FirstName', __(trans('base.FirstName')))->rules('required|min:2');
        $form->text('PatronymicName', __(trans('base.PatronymicName')));
        $form->text('Login', __(trans('base.Login')))->rules('required|min:6')->creationRules(['required', "unique:Lom_People"]);
        $form->password('Password', __(trans('base.Password')))->rules('required|min:6');

        return $form;
    }

    protected function renderAccessList(bool $enableCreation, $id_people)
    {
        $grid = new AccessGrid(new PeopleAccessInterim());
        $grid->disableExport();
        $grid->model()->where('id_People', '=', $id_people);
        $grid->option('show_actions', false);
        $grid->option('show_row_selector', false);
        if(!$enableCreation) $grid->disableCreation();

        $grid->model()->where('id_Access', '!=', 2);
               
        $grid->column(trans('base.Access'))->display(function()
        {
          $access = AccessList::where('id', $this->id_Access)->first();

          if($access != null) return $access->Comment;
          else return "Его не существует!";
        });

        $grid->column(trans('base.Option'))->display(function() use ($id_people)
        {
                  
          $delete = trans('admin.delete');
          $url = url('admin/peoples/'. $id_people .'/delete-access/'. $this->id);

          return <<<EOT

<div class="btn-group pull-right grid-create-btn" style="margin-right: 10px">
  <a href="{$url}" class="btn btn-sm btn-danger" title="{$delete}">
    <i class="fa fa-plus"></i><span class="hidden-xs">&nbsp;&nbsp;{$delete}</span>
  </a>
</div>
EOT;
        });

        return $grid;
    }
    
    
    protected function roleList($id_people, Content $content)
    {

      $grid = new Grid(new AccessList());
      $grid->disableCreation();
      $grid->disableExport();
      $grid->option('show_actions', false);
      $grid->option('show_row_selector', false);
      
      $grid->column('Comment', __(trans('base.Comment')));
      $grid->column(trans('base.Option'))->display(function() use ($id_people)
        {
                  
          $new = trans('admin.new');
          $url = url('admin/peoples/'. $id_people .'/add-access/'. $this->id);
          
          return <<<EOT

<div class="btn-group pull-right grid-create-btn" style="margin-right: 10px">
  <a href="{$url}" class="btn btn-sm btn-success" title="{$new}">
    <i class="fa fa-plus"></i><span class="hidden-xs">&nbsp;&nbsp;{$new}</span>
  </a>
</div>
EOT;
        });

        $cancel = trans('admin.cancel');
        $url = url('/admin/peoples/'. $id_people);
      
        $cancelButton = <<<EOT

<div class="btn-group pull-right grid-create-btn" style="margin-right: 10px">
  <a href="{$url}" class="btn btn-sm btn-danger" title="{$cancel}">
    <i class="fa fa-plus"></i><span class="hidden-xs">&nbsp;&nbsp;{$cancel}</span>
  </a>
</div>
EOT;       
      
        return $content
                ->title(trans('base.AddAccess'))
                ->body($grid->render() . $cancelButton);
    }

    protected function addAccess($id_people, $id_access)
    {
       $PeopleAccessInterim = new PeopleAccessInterim();
       $PeopleAccessInterim->id_People = $id_people;
       $PeopleAccessInterim->id_Access = $id_access;
       $PeopleAccessInterim->save();
       
       return redirect('admin/peoples/'. $id_people);
    }

    protected function deleteAccess($id_people, $id_accessInterim)
    {
        $PeopleAccessInterim = PeopleAccessInterim::find($id_accessInterim);
        $PeopleAccessInterim->delete();

        return redirect('admin/peoples/'. $id_people);
    }

    protected function renderOrganizationList(bool $enableCreation, $id_people)
    {
        $grid = new OrganizationGrid(new PeopleOrganizations());
        $grid->disableExport();
        $grid->model()->where('id_People', '=', $id_people);
        $grid->option('show_actions', false);
        $grid->option('show_row_selector', false);
        if(!$enableCreation) $grid->disableCreation();

        $grid->column(trans('base.Organization'))->display(function()
        {
          $organization = Organization::where('id', $this->id_Organizations)->first();

          if($organization != null) return $organization->Name;
          else return "Его не существует!";
        });
        
        $grid->column(trans('base.Option'))->display(function() use ($id_people)
        {
                  
          $delete = trans('admin.delete');
          $url = url('admin/peoples/'. $id_people .'/delete-organization/'. $this->id);

          return <<<EOT

<div class="btn-group pull-right grid-create-btn" style="margin-right: 10px">
  <a href="{$url}" class="btn btn-sm btn-danger" title="{$delete}">
    <i class="fa fa-plus"></i><span class="hidden-xs">&nbsp;&nbsp;{$delete}</span>
  </a>
</div>
EOT;
        });

        return $grid;
    }

    protected function organizationList($id_people, Content $content)
    {
      $grid = new Grid(new Organization());
      $grid->disableCreation();
      $grid->disableExport();
      $grid->option('show_actions', false);
      $grid->option('show_row_selector', false);
      
      $grid->column('Name', __(trans('base.Name')));
      $grid->column(trans('base.Option'))->display(function() use ($id_people)
        {
                  
          $new = trans('admin.new');
          $url = url('admin/peoples/'. $id_people .'/add-organization/'. $this->id);
          
          return <<<EOT

<div class="btn-group pull-right grid-create-btn" style="margin-right: 10px">
  <a href="{$url}" class="btn btn-sm btn-success" title="{$new}">
    <i class="fa fa-plus"></i><span class="hidden-xs">&nbsp;&nbsp;{$new}</span>
  </a>
</div>
EOT;
        });

        $cancel = trans('admin.cancel');
        $url = url('/admin/peoples/'. $id_people);
      
        $cancelButton = <<<EOT

<div class="btn-group pull-right grid-create-btn" style="margin-right: 10px">
  <a href="{$url}" class="btn btn-sm btn-danger" title="{$cancel}">
    <i class="fa fa-plus"></i><span class="hidden-xs">&nbsp;&nbsp;{$cancel}</span>
  </a>
</div>
EOT;       
      
        return $content
                ->title(trans('base.AddOrganization'))
                ->body($grid->render() . $cancelButton);
    }

    protected function addOrganization($id_people, $id_organization)
    {
       $PeopleOrganizations = new PeopleOrganizations();
       $PeopleOrganizations->id_People = $id_people;
       $PeopleOrganizations->id_Organizations = $id_organization;
       $PeopleOrganizations->save();
       
       return redirect('admin/peoples/'. $id_people);
    }

    protected function deleteOrganization($id_people, $id_peopleOrganization)
    {
      $PeopleOrganizations = PeopleOrganizations::find($id_peopleOrganization);
      $PeopleOrganizations->delete();

      return redirect('admin/peoples/'. $id_people);
    }
}
