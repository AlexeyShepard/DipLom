<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class EventsLogs extends Model
{
    /**
    * Таблица, связанная с моделью.
    *
    * @var string
    */
    protected $table = 'Lom_EventsLogs';

  /**
    * @var bool
    */
    public $timestamps = false;
}
