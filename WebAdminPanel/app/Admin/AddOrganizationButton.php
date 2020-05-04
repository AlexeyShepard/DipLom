<?php

namespace App\Admin;

use Encore\Admin\Grid\Tools\CreateButton;

class AddOrganizationButton extends CreateButton
{
    /**
     * Render CreateButton.
     *
     * @return string
     */
    public function render()
    {
        if (!$this->grid->showCreateBtn()) {
            return '';
        }

        $new = trans('admin.new');

        return <<<EOT

<div class="btn-group pull-right grid-create-btn" style="margin-right: 10px">
    <a href="{$this->grid->getCreateUrl()}" class="btn btn-sm btn-success" title="{$new}">
         <i class="fa fa-plus"></i><span class="hidden-xs">&nbsp;&nbsp;{$new}</span>
    </a>
</div>
EOT;

    }
}


?>