<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class PeopleOrganizations extends Model
{
    /**
    * Таблица, связанная с моделью.
    *
    * @var string
    */
    protected $table = 'Lom_PeopleOrganizations';

  /**
    * @var bool
    */
    public $timestamps = false;
}
