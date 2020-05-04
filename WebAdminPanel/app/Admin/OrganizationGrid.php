<?php

namespace App\Admin;

use Encore\Admin\Grid;

class OrganizationGrid extends Grid
{
    /**
     * Render create button for grid.
     *
     * @return string
     */
    public function renderCreateButton()
    {
        return (new AddOrganizationButton($this))->render();
    }
    
    
    /**
     * Get create url.
     *
     * @return string
     */
    public function getCreateUrl()
    {
        $queryString = '';

        if ($constraints = $this->model()->getConstraints()) {
            $queryString = http_build_query($constraints);
        }

        return sprintf('%s/add-organization%s',
            $this->resource(),
            $queryString ? ('?'.$queryString) : ''
        );
    }
}

?>