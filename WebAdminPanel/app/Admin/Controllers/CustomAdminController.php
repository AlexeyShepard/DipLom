<?php

namespace App\Admin\Controllers;

use Encore\Admin\Controllers\AdminController;

class CustomAdminController extends AdminController
{
    /**
     * Get content title.
     *
     * @return string
     */
    protected function title()
    {
        return trans('base.'. $this->title);
    }
}

?>